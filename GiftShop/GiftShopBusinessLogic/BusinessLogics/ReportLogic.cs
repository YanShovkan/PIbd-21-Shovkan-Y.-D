using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.HelperModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IMaterialStorage _materialStorage;
        private readonly IGiftStorage _giftStorage;
        private readonly IOrderStorage _orderStorage;
        public ReportLogic(IGiftStorage giftStorage, IMaterialStorage
      materialStorage, IOrderStorage orderStorage)
        {
            _giftStorage = giftStorage;
            _materialStorage = materialStorage;
            _orderStorage = orderStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>104
        public List<ReportGiftMaterialViewModel> GetGiftMaterial()
        {
            var materials = _materialStorage.GetFullList();
            var gifts = _giftStorage.GetFullList();
            var list = new List<ReportGiftMaterialViewModel>();
            foreach (var gift in gifts)
            {
                var record = new ReportGiftMaterialViewModel
                {
                    GiftName = gift.GiftName,
                    Materials = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var material in materials)
                {
                    if (gift.GiftMaterials.ContainsKey(material.Id))
                    {
                        record.Materials.Add(new Tuple<string, int>(material.MaterialName, gift.GiftMaterials[material.Id].Item2));
                        record.TotalCount += gift.GiftMaterials[material.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom =
           model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                GiftName = x.GiftName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
           .ToList();
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveMaterialsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список материалов",
                Gifts = _giftStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveGiftsMaterialsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список изделий",
                GiftMaterials = GetGiftMaterial()
            });
        }
        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }
}