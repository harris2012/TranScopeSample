using System;
using System.Collections.Generic;

namespace TranScopeSample.First.Entity
{

    /// <summary>
    /// `product_image`
    /// </summary>
    public class ProductImageEntity
    {

        /// <summary>
        /// int `product_image`.`id`
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// int `product_image`.`product_id`
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// int `product_image`.`count`
        /// </summary>
        public int? Count { get; set; }
    }
}
