using GiftShopBusinessLogic.Attributes;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public int Id { get; set; }
        [Column(title: "ФИО клиента", width: 100)]
        [DataMember]
        public string ClientFIO { get; set; }
        [Column(title: "Email", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string Email { get; set; }
        [Column(title: "Пароль", width: 100)]
        [DataMember]
        public string Password { get; set; }
    }
}