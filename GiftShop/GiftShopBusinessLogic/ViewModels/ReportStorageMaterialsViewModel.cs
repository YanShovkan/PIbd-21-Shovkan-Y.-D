using System;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ReportStorageMaterialsViewModel
    {
        public string StorageName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Materials { get; set; }
    }
}
