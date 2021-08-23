using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TranScopeSample.First.Entity;

namespace TranScopeSample.First
{

    /// <summary>
    /// FirstDBContext
    /// </summary>
    public partial class FirstDBContext : DbContext
    {

        /// <summary>
        /// table `product`
        /// </summary>
        public DbSet<ProductEntity> Products { get; set; }

        /// <summary>
        /// table `product_image`
        /// </summary>
        public DbSet<ProductImageEntity> ProductImages { get; set; }

        /// <summary>
        /// FirstDBContext
        /// </summary>
        public FirstDBContext(DbContextOptions<FirstDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductEntity>(BuildProduct);
            builder.Entity<ProductImageEntity>(BuildProductImage);
        }
    }
}
