using GiftShopBusinessLogic.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GiftShopDatabaseImplement.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Sum { get; set; }
        [Required] 
        public OrderStatus Status { get; set; }
        [Required] 
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public virtual Gift Gift { get; set; }
    }
}
