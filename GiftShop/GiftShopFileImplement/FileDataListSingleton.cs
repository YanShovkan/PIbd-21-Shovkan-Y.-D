using GiftShopBusinessLogic.Enums;
using GiftShopFileImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GiftShopFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;
        private readonly string MaterialFileName = "Material.xml";
        private readonly string OrderFileName = "Order.xml";
        private readonly string GiftFileName = "Gift.xml";
        private readonly string StorageFileName = "Storage.xml";
        private readonly string ClientFileName = "Client.xml";

        public List<Material> Materials { get; set; }
        public List<Order> Orders { get; set; }
        public List<Gift> Gifts { get; set; }
        public List<Storage> Storages { get; set; }
        public List<Client> Clients { get; set; }
        private FileDataListSingleton()
        {
            Materials = LoadMaterials();
            Orders = LoadOrders();
            Gifts = LoadGifts();
            Storages = LoadStorages();
            Clients = LoadClients();
        }
        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }
        ~FileDataListSingleton()
        {
            SaveMaterials();
            SaveOrders();
            SaveGifts();
            SaveStorages();
            SaveClients();
        }

        private List<Material> LoadMaterials()
        {
            var list = new List<Material>();
            if (File.Exists(MaterialFileName))
            {
                XDocument xDocument = XDocument.Load(MaterialFileName);
                var xElements = xDocument.Root.Elements("Material").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Material
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        MaterialName = elem.Element("MaterialName").Value
                    });
                }
            }
            return list;
        }

        private List<Order> LoadOrders()
        {
            var list = new List<Order>();
            if (File.Exists(OrderFileName))
            {
                XDocument xDocument = XDocument.Load(OrderFileName);
                var xElements = xDocument.Root.Elements("Order").ToList();
                foreach (var elem in xElements)
                {
                    OrderStatus status = (OrderStatus)0;
                    switch ((elem.Element("Status").Value))
                    {
                        case "Принят":
                            status = (OrderStatus)0;
                            break;
                        case "Выполняется":
                            status = (OrderStatus)1;
                            break;
                        case "Готов":
                            status = (OrderStatus)2;
                            break;
                        case "Оплачен":
                            status = (OrderStatus)3;
                            break;
                    }

                    Order order = new Order
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        GiftId = Convert.ToInt32(elem.Element("GiftId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value),
                        Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                        Status = status,
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value)
                    };

                    if (!string.IsNullOrEmpty(elem.Element("DateImplement").Value))
                    {
                        order.DateImplement = Convert.ToDateTime(elem.Element("DateImplement").Value);
                    }
                    list.Add(order);
                }
            }
            return list;
        }

        private List<Gift> LoadGifts()
        {
            var list = new List<Gift>();
            if (File.Exists(GiftFileName))
            {
                XDocument xDocument = XDocument.Load(GiftFileName);
                var xElements = xDocument.Root.Elements("Gift").ToList();
                foreach (var elem in xElements)
                {
                    var giftMaterials = new Dictionary<int, int>();
                    foreach (var materials in
                   elem.Element("GiftMaterials").Elements("GiftMaterial").ToList())
                    {
                        giftMaterials.Add(Convert.ToInt32(materials.Element("Key").Value),
                       Convert.ToInt32(materials.Element("Value").Value));
                    }
                    list.Add(new Gift
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        GiftName = elem.Element("GiftName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        GiftMaterials = giftMaterials
                    });
                }
            }
            return list;
        }

        private List<Storage> LoadStorages()
        {
            var list = new List<Storage>();

            if (File.Exists(StorageFileName))
            {
                XDocument xDocument = XDocument.Load(StorageFileName);

                var xElements = xDocument.Root.Elements("Storage").ToList();

                foreach (var storage in xElements)
                {
                    var storageMaterials = new Dictionary<int, int>();

                    foreach (var material in storage.Element("StorageMaterials").Elements("StorageMaterial").ToList())
                    {
                        storageMaterials.Add(Convert.ToInt32(material.Element("Key").Value), Convert.ToInt32(material.Element("Value").Value));
                    }

                    list.Add(new Storage
                    {
                        Id = Convert.ToInt32(storage.Attribute("Id").Value),
                        StorageName = storage.Element("StorageName").Value,
                        StorageManager = storage.Element("StorageManager").Value,
                        DateCreate = Convert.ToDateTime(storage.Element("DateCreate").Value),
                        StorageMaterials = storageMaterials
                    });
                }
            }

            return list;
        }

        private List<Client> LoadClients()
        {
            var list = new List<Client>();
            if (File.Exists(ClientFileName))
            {
                XDocument xDocument = XDocument.Load(ClientFileName);
                var xElements = xDocument.Root.Elements("Clients").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Client
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ClientFIO = elem.Element("ClientFIO").Value,
                        Email = elem.Element("Email").Value,
                        Password = elem.Element("Password").Value
                    });
                }
            }
            return list;
        }
        private void SaveMaterials()
        {
            if (Materials != null)
            {
                var xElement = new XElement("Materials");
                foreach (var material in Materials)
                {
                    xElement.Add(new XElement("Material",
                    new XAttribute("Id", material.Id),
                    new XElement("MaterialName", material.MaterialName)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(MaterialFileName);
            }
        }

        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");
                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                    new XAttribute("Id", order.Id),
                    new XElement("GiftId", order.GiftId),
                    new XElement("Count", order.Count),
                    new XElement("Sum", order.Sum),
                    new XElement("Status", order.Status),
                    new XElement("DateCreate", order.DateCreate),
                    new XElement("DateImplement", order.DateImplement)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }
       
        private void SaveGifts()
        {
            if (Gifts != null)
            {
                var xElement = new XElement("Gifts");
                foreach (var gift in Gifts)
                {
                    var materialElement = new XElement("GiftMaterials");
                    foreach (var material in gift.GiftMaterials)
                    {
                        materialElement.Add(new XElement("GiftMaterial",
                        new XElement("Key", material.Key),
                        new XElement("Value", material.Value)));
                    }
                    xElement.Add(new XElement("Gift",
                     new XAttribute("Id", gift.Id),
                     new XElement("GiftName", gift.GiftName),
                     new XElement("Price", gift.Price),
                     materialElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(GiftFileName);
            }
        }

        private void SaveStorages()
        {
            if (Storages != null)
            {
                var xElement = new XElement("Storages");

                foreach (var storage in Storages)
                {
                    var storageMaterials = new XElement("StorageMaterials");

                    foreach (var material in storage.StorageMaterials)
                    {
                        storageMaterials.Add(new XElement("StorageMaterial",
                            new XElement("Key", material.Key),
                            new XElement("Value", material.Value)));
                    }

                    xElement.Add(new XElement("Storage",
                        new XAttribute("Id", storage.Id),
                        new XElement("StorageName", storage.StorageName),
                        new XElement("StorageManager", storage.StorageManager),
                        new XElement("DateCreate", storage.DateCreate.ToString()),
                        storageMaterials));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(StorageFileName);
            }
        }

        private void SaveClients()
        {
            if (Clients != null)
            {
                var xElement = new XElement("Clients");
                foreach (var client in Clients)
                {
                    xElement.Add(new XElement("Client",
                    new XAttribute("Id", client.Id),
                    new XElement("ClientFIO", client.ClientFIO),
                    new XElement("Email", client.Email),
                    new XElement("Password", client.Password)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ClientFileName);
            }
        }
    }
}