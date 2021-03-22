using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ReportGiftMaterialViewModel
    {
        [DisplayName("Имя подарка")]
        public string GiftName { get; set; }
        [DisplayName("Итоговое количество")]
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Materials { get; set; }
    }
}

