using System;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ReportProductComponentViewModel
    {
        public string MaterialName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Gifts { get; set; }
    }
}

