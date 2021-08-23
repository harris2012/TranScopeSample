using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TranScopeSample.First.Entity;

namespace TranScopeSample.First
{

    partial class FirstDBContext
    {

        private void BuildProduct(EntityTypeBuilder<ProductEntity> productEntity)
        {
            productEntity.ToTable("product");

            productEntity.HasKey(table => table.Id)
                .HasName("PRIMARY");

            // `product`.`id`
            productEntity.Property(p => p.Id)
                .HasColumnName("id");

            // `product`.`count`
            productEntity.Property(p => p.Count)
                .HasColumnName("count");
        }
    }
}
