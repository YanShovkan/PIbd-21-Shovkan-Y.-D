using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiftShopDatabaseImplement.Implements
{
    public class GiftStorage : IGiftStorage
    {
        public List<GiftViewModel> GetFullList()
        {
            using (var context = new GiftShopDatabase())
            {
                return context.Gifts
                    .Include(rec => rec.GiftMaterials)
                    .ThenInclude(rec => rec.Material)
                    .ToList()
                    .Select(rec => new GiftViewModel
                    {
                        Id = rec.Id,
                        GiftName = rec.GiftName,
                        Price = rec.Price,
                        GiftMaterials = rec.GiftMaterials
                            .ToDictionary(recGiftMaterials => recGiftMaterials.MaterialId,
                            recGiftMaterials => (recGiftMaterials.Material?.MaterialName,
                            recGiftMaterials.Count))
                    })
                    .ToList();
            }
        }
        public List<GiftViewModel> GetFilteredList(GiftBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new GiftShopDatabase())
            {
                return context.Gifts
                    .Include(rec => rec.GiftMaterials)
                    .ThenInclude(rec => rec.Material)
                    .Where(rec => rec.GiftName.Contains(model.GiftName))
                    .ToList()
                    .Select(rec => new GiftViewModel
                    {
                        Id = rec.Id,
                        GiftName = rec.GiftName,
                        Price = rec.Price,
                        GiftMaterials = rec.GiftMaterials
                            .ToDictionary(recGiftMaterials => recGiftMaterials.MaterialId,
                            recGiftMaterials => (recGiftMaterials.Material?.MaterialName,
                            recGiftMaterials.Count))
                    })
                    .ToList();
            }
        }
        public GiftViewModel GetElement(GiftBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new GiftShopDatabase())
            {
                var gift = context.Gifts
                    .Include(rec => rec.GiftMaterials)
                    .ThenInclude(rec => rec.Material)
                    .FirstOrDefault(rec => rec.GiftName == model.GiftName ||
                    rec.Id == model.Id);

                return gift != null ?
                    new GiftViewModel
                    {
                        Id = gift.Id,
                        GiftName = gift.GiftName,
                        Price = gift.Price,
                        GiftMaterials = gift.GiftMaterials
                            .ToDictionary(recGiftMaterial => recGiftMaterial.MaterialId,
                            recGiftMaterial => (recGiftMaterial.Material?.MaterialName,
                            recGiftMaterial.Count))
                    } :
                    null;
            }
        }
        public void Insert(GiftBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Gift(), context);
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
        public void Update(GiftBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var gift = context.Gifts.FirstOrDefault(rec => rec.Id == model.Id);

                        if (gift == null)
                        {
                            throw new Exception("Подарок не найден");
                        }

                        CreateModel(model, gift, context);
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
        public void Delete(GiftBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                var material = context.Gifts.FirstOrDefault(rec => rec.Id == model.Id);

                if (material == null)
                {
                    throw new Exception("Материал не найден");
                }

                context.Gifts.Remove(material);
                context.SaveChanges();
            }
        }
        private Gift CreateModel(GiftBindingModel model, Gift gift, GiftShopDatabase context)
        {
            gift.GiftName = model.GiftName;
            gift.Price = model.Price;
            if (gift.Id == 0)
            {
                context.Gifts.Add(gift);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var giftMaterial = context.GiftMaterials
                    .Where(rec => rec.GiftId == model.Id.Value)
                    .ToList();

                context.GiftMaterials.RemoveRange(giftMaterial
                    .Where(rec => !model.GiftMaterials.ContainsKey(rec.GiftId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateMaterial in giftMaterial)
                {
                    updateMaterial.Count = model.GiftMaterials[updateMaterial.MaterialId].Item2;
                    model.GiftMaterials.Remove(updateMaterial.GiftId);
                }
                context.SaveChanges();
            }
            foreach (var giftMaterial in model.GiftMaterials)
            {
                context.GiftMaterials.Add(new GiftMaterial
                {
                    GiftId = gift.Id,
                    MaterialId = giftMaterial.Key,
                    Count = giftMaterial.Value.Item2
                });
                context.SaveChanges();
            }
            return gift;
        }
    }
}