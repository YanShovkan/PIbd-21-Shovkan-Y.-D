using System;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.BindingModels
{
    public class StorageBindingModel
    {
        public int? Id { get; set; }

        public string StorageName { get; set; }

        public string StorageManager { get; set; }

        public DateTime DateCreate { get; set; }

        public Dictionary<int, (string, int)> StorageMaterials { get; set; }
    }
}
