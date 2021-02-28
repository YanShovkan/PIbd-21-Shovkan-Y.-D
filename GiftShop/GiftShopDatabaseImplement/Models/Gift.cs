﻿using System.Collections.Generic;

namespace GiftShopDatabaseImplement.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string GiftName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> GiftMaterials { get; set; }
    }
}
