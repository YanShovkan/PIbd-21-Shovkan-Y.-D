using System.Collections.Generic;
using GiftShopBusinessLogic.ViewModels;

namespace GiftShopBusinessLogic.HelperModels
{
    class StorageExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportStorageMaterialsViewModel> StorageMaterials { get; set; }
    }
}
