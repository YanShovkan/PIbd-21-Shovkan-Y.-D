using System.ComponentModel.DataAnnotations;

namespace GiftShopDatabaseImplement.Models
{
    public class GiftMaterial
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public int MaterialId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Material Material { get; set; }
        public virtual Gift Gift { get; set; }
    }
}
