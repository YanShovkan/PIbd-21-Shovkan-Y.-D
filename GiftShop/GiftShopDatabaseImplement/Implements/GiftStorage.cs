using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopDatabaseImplement.Implements
{
	public class GiftStorage : IGiftStorage
	{
		private readonly FileDataDatabaseSingleton source;

		public GiftStorage()
		{
			source = FileDataDatabaseSingleton.GetInstance();
		}

		public List<GiftViewModel> GetFullList()
		{
			return source.Gifts
			.Select(CreateModel)
			.ToList();
		}

		public List<GiftViewModel> GetFilteredList(GiftBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			return source.Gifts
			.Where(rec => rec.GiftName.Contains(model.GiftName))
			.Select(CreateModel)
			.ToList();
		}

		public GiftViewModel GetElement(GiftBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			var gift = source.Gifts
			.FirstOrDefault(rec => rec.GiftName == model.GiftName || rec.Id
		   == model.Id);
			return gift != null ? CreateModel(gift) : null;
		}

		public void Insert(GiftBindingModel model)
		{
			int maxId = source.Gifts.Count > 0 ? source.Materials.Max(rec => rec.Id) : 0;
			var element = new Gift
			{
				Id = maxId + 1,
				GiftMaterials = new
		   Dictionary<int, int>()
			};
			source.Gifts.Add(CreateModel(model, element));
		}

		public void Update(GiftBindingModel model)
		{
			var element = source.Gifts.FirstOrDefault(rec => rec.Id == model.Id);
			if (element == null)
			{
				throw new Exception("Элемент не найден");
			}
			CreateModel(model, element);
		}

		public void Delete(GiftBindingModel model)
		{
			Gift element = source.Gifts.FirstOrDefault(rec => rec.Id == model.Id);
			if (element != null)
			{
				source.Gifts.Remove(element);
			}
			else
			{
				throw new Exception("Элемент не найден");
			}
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
			return new GiftViewModel
			{
				Id = gift.Id,
				GiftName = gift.GiftName,
				Price = gift.Price,
				GiftMaterials = gift.GiftMaterials
				 .ToDictionary(recPC => recPC.Key, recPC =>
				 (source.Materials.FirstOrDefault(recC => recC.Id ==
				recPC.Key)?.MaterialName, recPC.Value))
			};
		}
	}
}