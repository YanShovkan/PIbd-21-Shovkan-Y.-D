using System.Collections.Generic;
using GiftShopBusinessLogic.ViewModels;

namespace GiftShopBusinessLogic.HelperModels
{
    public class StorageWordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<StorageViewModel> Storages { get; set; }
    }
}
