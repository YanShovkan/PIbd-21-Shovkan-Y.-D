using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShopDatabaseImplement.Models
{
    public class Gift
    {
        public int Id { get; set; }
        [Required]
        public string GiftName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("GiftId")]
        public virtual List<GiftMaterial> GiftMaterials { get; set; }
        [ForeignKey("GiftId")]
        public virtual List<Order> Orders { get; set; }
    }
}
