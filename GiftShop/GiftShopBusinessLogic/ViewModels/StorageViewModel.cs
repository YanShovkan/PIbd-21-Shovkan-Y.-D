using GiftShopBusinessLogic.Attributes;
using GiftShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class StorageViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public int Id { get; set; }

        [Column(title: "Название склада", width: 150)]
        [DataMember]
        public string StorageName { get; set; }

        [Column(title: "Ответственный", width: 150)]
        [DataMember]
        public string StorageManager { get; set; }

        [Column(title: "Дата создания", width: 100, format: "D")]
        [DataMember]
        public DateTime DateCreate { get; set; }

        public Dictionary<int, (string, int)> StorageMaterials { get; set; }
    }
}
