using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ReportGiftMaterialViewModel
    {
        [DataMember]
        [DisplayName("Имя подарка")]
        public string GiftName { get; set; }
        [DataMember]
        [DisplayName("Итоговое количество")]
        public int TotalCount { get; set; }
        [DataMember]
        public List<Tuple<string, int>> Materials { get; set; }
    }
}

