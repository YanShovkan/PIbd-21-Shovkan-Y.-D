using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class GiftViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [DisplayName("Название подарка")]
        public string GiftName { get; set; }
        [DataMember]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> GiftMaterials { get; set; }
    }
}
