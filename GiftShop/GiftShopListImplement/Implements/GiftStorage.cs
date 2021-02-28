using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopListImplement.Implements
{
    public class GiftStorage : IGiftStorage
    {
        private readonly DataListSingleton source;
        public GiftStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<GiftViewModel> GetFullList()
        {
            List<GiftViewModel> result = new List<GiftViewModel>();
            foreach (var material in source.Gifts)
            {
                result.Add(CreateModel(material));
            }
            return result;
        }
        public List<GiftViewModel> GetFilteredList(GiftBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<GiftViewModel> result = new List<GiftViewModel>();
            foreach (var gift in source.Gifts)
            {
                if (gift.GiftName.Contains(model.GiftName))
                {
                    result.Add(CreateModel(gift));
                }
            }
            return result;
        }
        public GiftViewModel GetElement(GiftBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var gift in source.Gifts)
            {
                if (gift.Id == model.Id || gift.GiftName ==
                model.GiftName)
                {
                    return CreateModel(gift);
                }
            }
            return null;
        }
        public void Insert(GiftBindingModel model)
        {
            Gift tempGift = new Gift
            {
                Id = 1,
                GiftMaterials = new
            Dictionary<int, int>()
            };
            foreach (var gift in source.Gifts)
            {
                if (gift.Id >= tempGift.Id)
                {
                    tempGift.Id = gift.Id + 1;
                }
            }
            source.Gifts.Add(CreateModel(model, tempGift));
        }
        public void Update(GiftBindingModel model)
        {
            Gift tempGift = null;
            foreach (var gift in source.Gifts)
            {
                if (gift.Id == model.Id)
                {
                    tempGift = gift;
                }
            }
            if (tempGift == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempGift);
        }
        public void Delete(GiftBindingModel model)
        {
            for (int i = 0; i < source.Gifts.Count; ++i)
            {
                if (source.Gifts[i].Id == model.Id)
                {
                    source.Gifts.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Gift CreateModel(GiftBindingModel model, Gift gift)
        {
            gift.GiftName = model.GiftName;
            gift.Price = model.Price;
            // удаляем убранные
            foreach (var key in gift.GiftMaterials.Keys.ToList())
            {
                if (!model.GiftMaterials.ContainsKey(key))
                {
                    gift.GiftMaterials.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var material in model.GiftMaterials)
            {
                if (gift.GiftMaterials.ContainsKey(material.Key))
                {
                    gift.GiftMaterials[material.Key] =
                    model.GiftMaterials[material.Key].Item2;

                }
                else
                {
                    gift.GiftMaterials.Add(material.Key,
                    model.GiftMaterials[material.Key].Item2);
                }
            }
            return gift;
        }
        private GiftViewModel CreateModel(Gift gift)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> giftCMaterials = new
            Dictionary<int, (string, int)>();
            foreach (var pc in gift.GiftMaterials)
            {
                string materialName = string.Empty;
                foreach (var material in source.Materials)
                {
                    if (pc.Key == material.Id)
                    {
                        materialName = material.MaterialName;
                        break;
                    }
                }
                giftCMaterials.Add(pc.Key, (materialName, pc.Value));
            }
            return new GiftViewModel
            {
                Id = gift.Id,
                GiftName = gift.GiftName,
                Price = gift.Price,
                GiftMaterials = giftCMaterials
            };
        }
    }
}