using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.ViewModels;
using System.Collections.Generic;


namespace GiftShopBusinessLogic.Interfaces
{
    public interface IStorageStorage
    {
        List<StorageViewModel> GetFullList();

        List<StorageViewModel> GetFilteredList(StorageBindingModel model);

        StorageViewModel GetElement(StorageBindingModel model);

        void Insert(StorageBindingModel model);

        void Update(StorageBindingModel model);

        void Delete(StorageBindingModel model);
    }
}
