using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TranScopeSample.Second.Entity;

namespace TranScopeSample.Second
{

    /// <summary>
    /// SecondDBContext
    /// </summary>
    public partial class SecondDBContext : DbContext
    {

        /// <summary>
        /// table `order`
        /// </summary>
        public DbSet<OrderEntity> Orders { get; set; }

        /// <summary>
        /// SecondDBContext
        /// </summary>
        public SecondDBContext(DbContextOptions<SecondDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderEntity>(BuildOrder);
        }
    }
}
