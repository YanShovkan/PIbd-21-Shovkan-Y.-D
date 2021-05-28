using GiftShopBusinessLogic.Attributes;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class ImplementerViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public int Id { get; set; }
        [Column(title: "ФИО исполнителя", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string Name { get; set; }
        [Column(title: "Время на заказ", width: 100)]
        [DataMember]
        public int WorkingTime { get; set; }
        [Column(title: "Время на перерыв", width: 100)]
        [DataMember]
        public int PauseTime { get; set; }
    }
}
