using GiftShopBusinessLogic.Attributes;
using System;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class MessageInfoViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public string MessageId { get; set; }
        [Column(title: "Отправитель", width: 100)]
        [DataMember]
        public string SenderName { get; set; }
        [Column(title: "Дата письма", width: 100)]
        [DataMember]
        public DateTime DateDelivery { get; set; }
        [Column(title: "Заголовок", width: 100)]
        [DataMember]
        public string Subject { get; set; }
        [Column(title: "Текст", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string Body { get; set; }
    }
}
