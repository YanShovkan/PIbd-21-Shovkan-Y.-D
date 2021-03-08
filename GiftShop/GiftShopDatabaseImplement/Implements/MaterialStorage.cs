using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopDatabaseImplement.Implements
{
    public class MaterialStorage : IMaterialStorage
    {
        public List<MaterialViewModel> GetFullList()
        {
            using (var context = new GiftShopDatabase())
            {
                return context.Materials
                .Select(rec => new MaterialViewModel
                {
                    Id = rec.Id,
                    MaterialName = rec.MaterialName
                })
               .ToList();
            }
        }
        public List<MaterialViewModel> GetFilteredList(MaterialBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new GiftShopDatabase())
            {
                return context.Materials
                .Where(rec => rec.MaterialName.Contains(model.MaterialName))
               .Select(rec => new MaterialViewModel
               {
                   Id = rec.Id,
                   MaterialName = rec.MaterialName
               })
                .ToList();
            }
        }
        public MaterialViewModel GetElement(MaterialBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new GiftShopDatabase())
            {
                var component = context.Materials
                .FirstOrDefault(rec => rec.MaterialName == model.MaterialName ||
               rec.Id == model.Id);
                return component != null ?
                new MaterialViewModel
                {
                    Id = component.Id,
                    MaterialName = component.MaterialName
                } :
               null;
            }
        }
        public void Insert(MaterialBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                context.Materials.Add(CreateModel(model, new Material()));
                context.SaveChanges();
            }
        }
        public void Update(MaterialBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                var element = context.Materials.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
            {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(MaterialBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                Material element = context.Materials.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Materials.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Material CreateModel(MaterialBindingModel model, Material material)
        {
            material.MaterialName = model.MaterialName;
            return material;
        }
    }
}
