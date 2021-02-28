using System.Collections.Generic;
using System.ComponentModel;

namespace GiftShopBusinessLogic.ViewModels
{
    public class GiftViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название подарка")]
        public string GiftName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> GiftMaterials { get; set; }
    }
}
