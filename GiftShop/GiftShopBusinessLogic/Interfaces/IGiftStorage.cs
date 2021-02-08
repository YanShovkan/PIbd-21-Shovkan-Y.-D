using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.Interfaces
{
    public interface IGiftStorage
    {
        List<GiftViewModel> GetFullList();
        List<GiftViewModel> GetFilteredList(GiftBindingModel model);
        GiftViewModel GetElement(GiftBindingModel model);
        void Insert(GiftBindingModel model);
        void Update(GiftBindingModel model);
        void Delete(GiftBindingModel model);
 }
}