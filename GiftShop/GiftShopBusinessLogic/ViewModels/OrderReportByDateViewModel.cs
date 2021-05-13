using System;

namespace GiftShopBusinessLogic.ViewModels
{
    public class OrderReportByDateViewModel
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
