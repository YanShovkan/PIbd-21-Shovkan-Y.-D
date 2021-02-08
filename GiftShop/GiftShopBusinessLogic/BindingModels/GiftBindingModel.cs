using System.Collections.Generic;

namespace GiftShopBusinessLogic.BindingModels
{
    public class GiftBindingModel
    {
        public int? Id { get; set; }
        public string GiftName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> GiftComponents { get; set; }
    }
}
