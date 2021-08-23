using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TranScopeSample.First.Entity;

namespace TranScopeSample.First
{

    partial class FirstDBContext
    {

        private void BuildProductImage(EntityTypeBuilder<ProductImageEntity> productImageEntity)
        {
            productImageEntity.ToTable("product_image");

            productImageEntity.HasKey(table => table.Id)
                .HasName("PRIMARY");

            // `product_image`.`id`
            productImageEntity.Property(p => p.Id)
                .HasColumnName("id");

            // `product_image`.`product_id`
            productImageEntity.Property(p => p.ProductId)
                .HasColumnName("product_id");

            // `product_image`.`count`
            productImageEntity.Property(p => p.Count)
                .HasColumnName("count");
        }
    }
}
