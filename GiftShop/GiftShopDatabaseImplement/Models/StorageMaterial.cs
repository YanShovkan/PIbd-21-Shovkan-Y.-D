using System.ComponentModel.DataAnnotations;

namespace GiftShopDatabaseImplement.Models
{
    public class StorageMaterial
    {
        public int Id { get; set; }

        public int StorageId { get; set; }

        public int MaterialId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Material Material { get; set; }

        public virtual Storage Storage { get; set; }
    }
}
