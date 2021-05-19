﻿using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.Interfaces;
using GiftShopBusinessLogic.ViewModels;
using GiftShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopDatabaseImplement.Implements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        public List<MessageInfoViewModel> GetFullList()
        {
            using (var context = new GiftShopDatabase())
            {
                return context.Messages
                .Select(rec => new MessageInfoViewModel
                {
                    MessageId = rec.MessageId,
                    SenderName = rec.SenderName,
                    DateDelivery = rec.DateDelivery,
                    Subject = rec.Subject,
                    Body = rec.Body
                })
                .ToList();
            }
        }
        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new GiftShopDatabase())
            {
                if (model.ToSkip.HasValue && model.ToTake.HasValue && !model.ClientId.HasValue)
                {
                    return context.Messages.Skip((int)model.ToSkip).Take((int)model.ToTake)
                    .Select(CreateModel).ToList();
                }
                return context.Messages
                .Where(rec => (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                (!model.ClientId.HasValue && rec.DateDelivery.Date == model.DateDelivery.Date))
                .Skip(model.ToSkip ?? 0)
                .Take(model.ToTake ?? context.Messages.Count())
                .Select(CreateModel)
                .ToList();
            }
        }
        public void Insert(MessageInfoBindingModel model)
        {
            using (var context = new GiftShopDatabase())
            {
                MessageInfo element = context.Messages.FirstOrDefault(rec => rec.MessageId == model.MessageId);
                if (element != null)
                {
                    throw new Exception("Уже есть письмо с таким идентификатором");
                }
                context.Messages.Add(new MessageInfo
                {
                    MessageId = model.MessageId,
                    ClientId = model.ClientId,
                    SenderName = model.FromMailAddress,
                    DateDelivery = model.DateDelivery,
                    Subject = model.Subject,
                    Body = model.Body
                });
                context.SaveChanges();
            }
        }
        private MessageInfoViewModel CreateModel(MessageInfo model)
        {
            return new MessageInfoViewModel
            {
                MessageId = model.MessageId,
                SenderName = model.SenderName,
                DateDelivery = model.DateDelivery,
                Subject = model.Subject,
                Body = model.Body
            };
        }
    }
}