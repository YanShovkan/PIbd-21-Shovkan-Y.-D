using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace GiftShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientLogic _clientLogic;
        private readonly MailLogic _mailLogic;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;
        private readonly int mailsOnPage = 2;
        public ClientController(ClientLogic clientLogic, MailLogic mailLogic)
        {
            _clientLogic = clientLogic;
            _mailLogic = mailLogic;
            if (mailsOnPage < 1) { mailsOnPage = 5; }
        }
        [HttpGet]
        public ClientViewModel Login(string login, string password) => _clientLogic.Read(new ClientBindingModel { Email = login, Password = password })?[0];
        [HttpPost]
        public void Register(ClientBindingModel model)
        {
            CheckData(model);
            _clientLogic.CreateOrUpdate(model);
        }
        [HttpGet]
        public (List<MessageInfoViewModel>, bool) GetMessages(int clientId, int page)
        {
            var list = _mailLogic.Read(new MessageInfoBindingModel { ClientId = clientId, ToSkip = (page - 1) * mailsOnPage, ToTake = mailsOnPage + 1 }).ToList();
            var hasNext = !(list.Count() <= mailsOnPage);
            return (list.Take(mailsOnPage).ToList(), hasNext);
        }
        [HttpPost]
        public void UpdateData(ClientBindingModel model)
        {
            CheckData(model);
            _clientLogic.CreateOrUpdate(model);
        }
        private void CheckData(ClientBindingModel model)
        {
            if (!Regex.IsMatch(model.Email, @"^[A-Za-z0-9]+(?:[._%+-])?[A-Za-z0-9._-]+[A-Za-z0-9]@[A-Za-z0-9]+(?:[.-])?[A-Za-z0-9._-]+\.[A-Za-z]{2,6}$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
            if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength || !Regex.IsMatch(model.Password, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль длиной от {_passwordMinLength} до {_passwordMaxLength} должен состоять из цифр, букв и небуквенных символов");
            }
        }
    }
}