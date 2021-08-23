using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panosen.Transactions;
using TranScopeSample.First;
using TranScopeSample.Second;

namespace TranScopeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddTransient<TestCase>();

            services.AddDbContext<FirstDBContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("FirstDB"));
            });

            services.AddDbContext<SecondDBContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("SecondDB"));
            });

            var serviceProvider = services.BuildServiceProvider();

            //如果多个SaveChanges方法提交，则需用IDbContextTransaction
            {
                var testCase = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TestCase>();
                testCase.SingleDBContext(101);
            }

            //如果是多个Context，即分布式事务，则需用TransactionScope
            {
                var testCase = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TestCase>();
                testCase.MultiDBContext(301);
            }
            {
                var testCase = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TestCase>();
                testCase.MultiDBContext(302);
            }

            Console.WriteLine("okok");
        }
    }

    public class TestCase
    {
        private readonly FirstDBContext firstDBContext;
        private readonly SecondDBContext secondDBContext;

        public TestCase(FirstDBContext firstDBContext, SecondDBContext secondDBContext)
        {
            this.firstDBContext = firstDBContext;
            this.secondDBContext = secondDBContext;
        }

        /// <summary>
        /// 如果多个SaveChanges方法提交，则需用IDbContextTransaction
        /// </summary>
        public void SingleDBContext(int value)
        {
            using (var trans = firstDBContext.Database.BeginTransaction())
            {
                var product = new First.Entity.ProductEntity { Count = value };
                firstDBContext.Products.Add(product);
                firstDBContext.SaveChanges();

                var productImage = new First.Entity.ProductImageEntity { ProductId = product.Id, Count = value };
                firstDBContext.ProductImages.Add(productImage);
                firstDBContext.SaveChanges();

                trans.Commit();
            }
        }

        /// <summary>
        /// 二阶段提交事务
        /// </summary>
        public void MultiDBContext(int value)
        {
            using (var transaction = new Transaction())
            {
                var productEntity = new First.Entity.ProductEntity
                {
                    Count = value
                };

                transaction.Prepare(() =>
                {
                    firstDBContext.Products.Add(productEntity);
                    firstDBContext.SaveChanges();

                    return true;
                }).OnCommit(() =>
                {
                    return true;
                }).OnRollback(() =>
                {
                    firstDBContext.Products.Remove(productEntity);
                    firstDBContext.SaveChanges();
                });

                var orderEntity = new Second.Entity.OrderEntity
                {
                    Count = value
                };
                transaction.Prepare(() =>
                {
                    secondDBContext.Orders.Add(orderEntity);
                    secondDBContext.SaveChanges();

                    return true;
                }).OnCommit(() =>
                {
                    //当value 等于 302时，使此提交失败
                    return value != 302;
                }).OnRollback(() =>
                {
                    secondDBContext.Orders.Remove(orderEntity);
                    secondDBContext.SaveChanges();
                });
            }
        }
    }
}