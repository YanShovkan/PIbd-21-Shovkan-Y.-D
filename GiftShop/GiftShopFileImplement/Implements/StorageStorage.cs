using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopFileImplement.Models;

namespace GiftShopFileImplement.Implements
{
    public class StorageStorage : IStorageStorage
    {
        private readonly FileDataListSingleton source;

        public StorageStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        private Storage CreateModel(StorageBindingModel model, Storage storage)
        {
            storage.StorageName = model.StorageName;
            storage.StorageManager = model.StorageManager;

            foreach (var key in storage.StorageMaterials.Keys.ToList())
            {
                if (!model.StorageMaterials.ContainsKey(key))
                {
                    storage.StorageMaterials.Remove(key);
                }
            }

            foreach (var material in model.StorageMaterials)
            {
                if (storage.StorageMaterials.ContainsKey(material.Key))
                {
                    storage.StorageMaterials[material.Key] =
                        model.StorageMaterials[material.Key].Item2;
                }
                else
                {
                    storage.StorageMaterials.Add(material.Key, model.StorageMaterials[material.Key].Item2);
                }
            }

            return storage;
        }

        private StorageViewModel CreateModel(Storage storage)
        {
            Dictionary<int, (string, int)> storageMaterials = new Dictionary<int, (string, int)>();

            foreach (var storageMaterial in storage.StorageMaterials)
            {
                string materialName = string.Empty;
                foreach (var material in source.Materials)
                {
                    if (storageMaterial.Key == material.Id)
                    {
                        materialName = material.MaterialName;
                        break;
                    }
                }
                storageMaterials.Add(storageMaterial.Key, (materialName, storageMaterial.Value));
            }

            return new StorageViewModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                StorageManager = storage.StorageManager,
                DateCreate = storage.DateCreate,
                StorageMaterials = storageMaterials
            };
        }

        public List<StorageViewModel> GetFullList()
        {
            return source.Storages.Select(CreateModel).ToList();
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return source.Storages
                .Where(xStorage => xStorage.StorageName
                .Contains(model.StorageName))
                .Select(CreateModel).ToList();
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var storage = source.Storages.
                FirstOrDefault(xStorage => xStorage.StorageName == model.StorageName || xStorage.Id == model.Id);

            return storage != null ? CreateModel(storage) : null;
        }

        public void Insert(StorageBindingModel model)
        {
            int maxId = source.Storages.Count > 0 ? source.Storages.Max(xStorage => xStorage.Id) : 0;
            var storage = new Storage { Id = maxId + 1, StorageMaterials = new Dictionary<int, int>(), DateCreate = DateTime.Now };
            source.Storages.Add(CreateModel(model, storage));
        }

        public void Update(StorageBindingModel model)
        {
            var storage = source.Storages.FirstOrDefault(XStorage => XStorage.Id == model.Id);

            if (storage == null)
            {
                throw new Exception("Склад не найден");
            }

            CreateModel(model, storage);
        }

        public void Delete(StorageBindingModel model)
        {
            var storage = source.Storages.FirstOrDefault(XStorage => XStorage.Id == model.Id);

            if (storage != null)
            {
                source.Storages.Remove(storage);
            }
            else
            {
                throw new Exception("Склад не найден");
            }
        }

        public bool CheckMaterials(GiftViewModel model, int materialCountInOrder)
        {
            throw new NotImplementedException();
        }
    }
}