using System.Collections.Generic;
using GiftShopBusinessLogic.ViewModels;

namespace GiftShopBusinessLogic.HelperModels
{
    class PdfInfoOrderReportByDate
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<OrderReportByDateViewModel> Orders { get; set; }
    }
}
