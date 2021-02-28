using GiftShopListImplement.Models;
using System.Collections.Generic;

namespace GiftShopListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Material> Materials { get; set; }
        public List<Order> Orders { get; set; }
        public List<Gift> Gifts { get; set; }
        private DataListSingleton()
        {
            Materials = new List<Material>();
            Orders = new List<Order>();
            Gifts = new List<Gift>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}