using System;
using System.Collections.Generic;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;

namespace GiftShopBusinessLogic.BusinessLogics
{
    public class StorageLogic
    {
        private readonly IStorageStorage _storageStorage;

        private readonly IMaterialStorage _materialStorage;

        public StorageLogic(IStorageStorage storage, IMaterialStorage materialStorage)
        {
            _storageStorage = storage;
            _materialStorage = materialStorage;
        }

        public List<StorageViewModel> Read(StorageBindingModel model)
        {
            if (model == null)
            {
                return _storageStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<StorageViewModel>
                {
                    _storageStorage.GetElement(model)
                };
            }

            return _storageStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(StorageBindingModel model)
        {
            var element = _storageStorage.GetElement(new StorageBindingModel
            {
                StorageName = model.StorageName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть склад с таким названием");
            }

            if (model.Id.HasValue)
            {
                _storageStorage.Update(model);
            }
            else
            {
                _storageStorage.Insert(model);
            }
        }

        public void Delete(StorageBindingModel model)
        {
            var element = _storageStorage.GetElement(new StorageBindingModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            _storageStorage.Delete(model);
        }

        public void Replenishment(ReplenishStorageBindingModel model)
        {
            var storage = _storageStorage.GetElement(new StorageBindingModel
            {
                Id = model.StorageId
            });

            var material = _materialStorage.GetElement(new MaterialBindingModel
            {
                Id = model.MaterialId
            });

            if (storage == null)
            {
                throw new Exception("Не найден склад");
            }

            if (material == null)
            {
                throw new Exception("Не найден материал");
            }

            if (storage.StorageMaterials.ContainsKey(model.MaterialId))
            {
                storage.StorageMaterials[model.MaterialId] = (material.MaterialName, storage.StorageMaterials[model.MaterialId].Item2 + model.Count);
            }
            else
            {
                storage.StorageMaterials.Add(material.Id, (material.MaterialName, model.Count));
            }

            _storageStorage.Update(new StorageBindingModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                StorageManager = storage.StorageManager,
                DateCreate = storage.DateCreate,
                StorageMaterials = storage.StorageMaterials
            });
        }
    }
}
