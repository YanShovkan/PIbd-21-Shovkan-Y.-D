using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftShopBusinessLogic.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string StorageName { get; set; }

        [DisplayName("ФИО ответственного")]
        public string StorageManager { get; set; }

        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }

        public Dictionary<int, (string, int)> StorageMaterials { get; set; }
    }
}
