using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace GiftShopBusinessLogic.Interfaces
{
    public interface IMessageInfoStorage
    {
        List<MessageInfoViewModel> GetFullList();
        List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model);
        void Insert(MessageInfoBindingModel model);
    }
}
