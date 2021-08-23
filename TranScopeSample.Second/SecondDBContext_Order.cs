using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TranScopeSample.Second.Entity;

namespace TranScopeSample.Second
{

    partial class SecondDBContext
    {

        private void BuildOrder(EntityTypeBuilder<OrderEntity> orderEntity)
        {
            orderEntity.ToTable("order");

            orderEntity.HasKey(table => table.Id)
                .HasName("PRIMARY");

            // `order`.`id`
            orderEntity.Property(p => p.Id)
                .HasColumnName("id");

            // `order`.`count`
            orderEntity.Property(p => p.Count)
                .HasColumnName("count");
        }
    }
}
