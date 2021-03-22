using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ReportProductComponentViewModel
    {
        [DisplayName("Имя метериала")]
        public string MaterialName { get; set; }
        [DisplayName("Итоговое количество")]
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Gifts { get; set; }
    }
}

