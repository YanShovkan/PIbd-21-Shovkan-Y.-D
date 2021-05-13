using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GiftShopBusinessLogic.ViewModels;
using GiftShopBusinessLogic.BindingModels;
using GiftShopStorageManagerApp.Models;

namespace GiftShopStorageManagerApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (Program.Enter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIStorageManager.GetRequest<List<StorageViewModel>>("api/storage/GetStorageList"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (password != Program.CurrentPassword)
                {
                    throw new Exception("Invalid password");
                }
                Program.Enter = true;
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Enter Password");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void Create(string storageName, string managerName)
        {
            if (!string.IsNullOrEmpty(storageName) && !string.IsNullOrEmpty(managerName))
            {
                APIStorageManager.PostRequest("api/storage/CreateOrUpdateStorage", new StorageBindingModel
                {
                    StorageManager = managerName,
                    StorageName = storageName,
                    DateCreate = DateTime.Now,
                    StorageMaterials = new Dictionary<int, (string, int)>()
                });
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Type in responsible person full name");
        }

        [HttpGet]
        public IActionResult Update(int storageId)
        {
            var storage = APIStorageManager.GetRequest<StorageViewModel>($"api/storage/GetStorage?storageId={storageId}");
            ViewBag.StorageMaterials = storage.StorageMaterials.Values;
            ViewBag.StorageName = storage.StorageName;
            ViewBag.StorageManager = storage.StorageManager;
            return View();
        }

        [HttpPost]
        public void Update(int storageId, string storageName, string managerName)
        {
            if (!string.IsNullOrEmpty(storageName) && !string.IsNullOrEmpty(managerName))
            {
                var storage = APIStorageManager.GetRequest<StorageViewModel>($"api/storage/GetStorage?storageId={storageId}");
                if (storage == null)
                {
                    return;
                }
                APIStorageManager.PostRequest("api/storage/CreateOrUpdateStorage", new StorageViewModel
                {
                    StorageManager = managerName,
                    StorageName = storageName,
                    DateCreate = DateTime.Now,
                    StorageMaterials = storage.StorageMaterials,
                    Id = storage.Id
                });
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Enter login, password and full name");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            if (Program.Enter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Storage = APIStorageManager.GetRequest<List<StorageViewModel>>("api/storage/GetStorageList");
            return View();
        }

        [HttpPost]
        public void Delete(int storageId)
        {
            APIStorageManager.PostRequest("api/storage/DeleteStorage", new StorageBindingModel
            {
                Id = storageId
            });
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Replenishment()
        {
            if (Program.Enter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Storage = APIStorageManager.GetRequest<List<StorageViewModel>>("api/storage/GetStorageList");
            ViewBag.Material = APIStorageManager.GetRequest<List<MaterialViewModel>>("api/storage/GetMaterialList");
            return View();
        }

        [HttpPost]
        public void Replenishment(int storageId, int materialId, int count)
        {
            APIStorageManager.PostRequest("api/storage/Replenishment", new ReplenishStorageBindingModel
            {
                StorageId = storageId,
                MaterialId = materialId,
                Count = count
            });
            Response.Redirect("Replenishment");
        }
    }
}
