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
        private readonly IStorageStorage _storageStorage;
        private readonly IGiftStorage _giftStorage;
        private readonly IOrderStorage _orderStorage;

        public ReportLogic(IGiftStorage giftStorage, IStorageStorage
      storageStorage, IOrderStorage orderStorage)
        {
            _giftStorage = giftStorage;
            _storageStorage = storageStorage;
            _orderStorage = orderStorage;
        }
        
        public List<ReportGiftMaterialViewModel> GetGiftMaterial()
        {
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
                foreach (var material in gift.GiftMaterials)
                { 
                    record.Materials.Add(new Tuple<string, int>(material.Value.Item1, material.Value.Item2));
                    record.TotalCount += material.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }

        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
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

        public void SaveMaterialsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список подарков",
                Gifts = _giftStorage.GetFullList()
            });
        }

        public void SaveGiftsMaterialsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список материалов",
                GiftMaterials = GetGiftMaterial()
            });
        }
 
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

        public List<ReportStorageMaterialsViewModel> GetStorageMaterials()
        {
            var storages = _storageStorage.GetFullList();
            var list = new List<ReportStorageMaterialsViewModel>();
            foreach (var storage in storages)
            {
                var record = new ReportStorageMaterialsViewModel
                {
                    StorageName = storage.StorageName,
                    Materials = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var material in storage.StorageMaterials)
                {
                    record.Materials.Add(new Tuple<string, int>(material.Value.Item1, material.Value.Item2));
                    record.TotalCount += material.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }

        public List<OrderReportByDateViewModel> GetOrderReportByDate()
        {
            return _orderStorage.GetFullList()
                .GroupBy(order => order.DateCreate.ToShortDateString())
                .Select(rec => new OrderReportByDateViewModel
                {
                    Date = Convert.ToDateTime(rec.Key),
                    Count = rec.Count(),
                    Sum = rec.Sum(order => order.Sum)
                })
                .ToList();
        }

        public void SaveStoragesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateStoragesDoc(new StorageWordInfo
            {
                FileName = model.FileName,
                Title = "Список складов",
                Storages = _storageStorage.GetFullList()
            });
        }

        public void SaveStorageMaterialsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateStoragesDoc(new StorageExcelInfo
            {
                FileName = model.FileName,
                Title = "Список загруженности складов",
                StorageMaterials = GetStorageMaterials()
            });
        }

        public void SaveOrderReportByDateToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDocOrderReportByDate(new PdfInfoOrderReportByDate
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrderReportByDate()
            });
        }

    }
}