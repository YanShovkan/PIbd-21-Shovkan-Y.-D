using GiftShopBusinessLogic.BindingModels;
using GiftShopBusinessLogic.BusinessLogics;
using GiftShopBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GiftShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;

        private readonly GiftLogic _gift;

        private readonly OrderLogic _main;

        public MainController(OrderLogic order, GiftLogic gift, OrderLogic main)
        {
            _order = order;
            _gift = gift;
            _main = main;
        }

        [HttpGet]
        public List<GiftViewModel> GetGiftList() => _gift.Read(null)?.ToList();

        [HttpGet]
        public GiftViewModel GetGift(int giftId) => _gift.Read(new GiftBindingModel { Id = giftId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}