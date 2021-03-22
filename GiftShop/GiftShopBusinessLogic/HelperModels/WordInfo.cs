using GiftShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<MaterialViewModel> Maetrials { get; set; }
    }
}
