using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StorageController : Controller
    {
        private readonly StorageLogic storageLogic;
        private readonly MaterialLogic materialLogic;

        public StorageController(StorageLogic storageLogic, MaterialLogic materialLogic)
        {
            this.storageLogic = storageLogic;
            this.materialLogic = materialLogic;
        }

        [HttpGet]
        public List<StorageViewModel> GetStorageList() => storageLogic.Read(null)?.ToList();

        [HttpPost]
        public void CreateOrUpdateStorage(StorageBindingModel model) => storageLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteStorage(StorageBindingModel model) => storageLogic.Delete(model);

        [HttpPost]
        public void Replenishment(ReplenishStorageBindingModel model) => storageLogic.Replenishment(model);

        [HttpGet]
        public StorageViewModel GetStorage(int storageId) => storageLogic.Read(new StorageBindingModel { Id = storageId })?[0];

        [HttpGet]
        public List<MaterialViewModel> GetMaterialList() => materialLogic.Read(null);
    }
}