using System;
using System.Collections.Generic;
using System.Text;

namespace GiftShopListImplement.Models
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorageName { get; set; }

        public string StorageManager { get; set; }

        public DateTime DateCreate { get; set; }

        public Dictionary<int, int> StorageMaterials { get; set; }
    }
}
