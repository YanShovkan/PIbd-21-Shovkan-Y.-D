using System;
using System.Collections.Generic;
using System.Linq;
using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftShopDatabaseImplement.Implements

{
    public class StorageStorage : IStorageStorage
    {
        public List<StorageViewModel> GetFullList()
        {
            using (var context = new GiftShopDatabase())
            {
                return context.Storages
                    .Include(rec => rec.StorageMaterials)
                    .ThenInclude(rec => rec.Material)
                    .ToList().Select(CreateModel)
                    .ToList();
            }
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new GiftShopDatabase())
            {
                return context.Storages
                    .Include(rec => rec.StorageMaterials)
                    .ThenInclude(rec => rec.Material)
                    .Where(rec => rec.StorageName
                    .Contains(model.StorageName))
                    .ToList()
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new GiftShopDatabase())
            {
                var storage = context.Storages
                    .Include(rec => rec.StorageMaterials)
                    .ThenInclude(rec => rec.Material)
                    .FirstOrDefault(rec => rec.StorageName == model.StorageName || rec.Id == model.Id);

                return storage != null ?
                    CreateModel(storage) :
                    null;
            }
        }

        public void Insert(StorageBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Storage(), context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(StorageBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Storages.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(StorageBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                Storage element = context.Storages.FirstOrDefault(rec => rec.Id == model.Id);

                if (element != null)
                {
                    context.Storages.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private StorageViewModel CreateModel(Storage storage)
        {
            return new StorageViewModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                StorageManager = storage.StorageManager,
                DateCreate = storage.DateCreate,
                StorageMaterials = storage.StorageMaterials
                            .ToDictionary(recPC => recPC.MaterialId, recPC => (recPC.Material?.MaterialName, recPC.Count))
            };
        }
        private Storage CreateModel(StorageBindingModel model, Storage storage, GiftShopDatabase context)
        {
            storage.StorageName = model.StorageName;
            storage.StorageManager = model.StorageManager;
            storage.DateCreate = model.DateCreate;

            if (storage.Id == 0)
            {
                context.Storages.Add(storage);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var storageMaterials = context.StorageMaterials
                    .Where(rec => rec.StorageId == model.Id.Value)
                    .ToList();

                context.StorageMaterials
                    .RemoveRange(storageMaterials
                        .Where(rec => !model.StorageMaterials
                            .ContainsKey(rec.MaterialId))
                                .ToList());
                context.SaveChanges();

                foreach (var updateMaterial in storageMaterials)
                {
                    updateMaterial.Count = model.StorageMaterials[updateMaterial.MaterialId].Item2;
                    model.StorageMaterials.Remove(updateMaterial.MaterialId);
                }
                context.SaveChanges();
            }

            foreach (var material in model.StorageMaterials)
            {
                context.StorageMaterials.Add(new StorageMaterial
                {
                    StorageId = storage.Id,
                    MaterialId = material.Key,
                    Count = material.Value.Item2
                });

                context.SaveChanges();
            }

            return storage;
        }

        public bool CheckMaterials(GiftViewModel model, int materialCountInOrder)
        {
            using (var context = new GiftShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {

                    foreach (var materialsInGift in model.GiftMaterials)
                    {
                        int materialsCountInGift = materialsInGift.Value.Item2 * materialCountInOrder;

                        List<StorageMaterial> oneOfMaterial = context.StorageMaterials
                            .Where(storehouse => storehouse.MaterialId == materialsInGift.Key)
                            .ToList();

                        foreach (var material in oneOfMaterial)
                        {
                            int materialCountInStorage = material.Count;

                            if (materialCountInStorage <= materialsCountInGift)
                            {
                                materialsCountInGift -= materialCountInStorage;
                                context.Storages.FirstOrDefault(rec => rec.Id == material.StorageId).StorageMaterials.Remove(material);
                            }
                            else
                            {
                                material.Count -= materialsCountInGift;
                                materialsCountInGift = 0;
                            }

                            if (materialsCountInGift == 0)
                            {
                                break;
                            }
                        }

                        if (materialsCountInGift > 0)
                        {
                            transaction.Rollback();

                            return false;
                        }
                    }

                    context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
            }
        }
    }
}