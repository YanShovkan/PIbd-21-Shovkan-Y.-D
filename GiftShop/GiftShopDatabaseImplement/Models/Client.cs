using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShopDatabaseImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ClientFIO { get; set; }

        [ForeignKey("ClientId")]
        public virtual List<Order> Orders { get; set; }
        [ForeignKey("ClientId")]
        public List<MessageInfo> Messages { get; set; }
    }
}