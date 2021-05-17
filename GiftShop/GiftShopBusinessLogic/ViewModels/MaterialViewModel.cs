using GiftShopBusinessLogic.Attributes;
using System.Runtime.Serialization;

namespace GiftShopBusinessLogic.ViewModels
{
    public class MaterialViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public int Id { get; set; }
        [Column(title: "Название", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string MaterialName { get; set; }
    }
}
