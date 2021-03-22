using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShopDatabaseImplement.Models
{
    public class Material
    {
        public int Id { get; set; }
        
        [Required]
        public string MaterialName { get; set; }
        
        [ForeignKey("MaterialId")]
        public virtual List<GiftMaterial> GiftMaterials { get; set; }
    }
}
