using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.DTOs;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.DataAccess.Utils;
using FUMiniHotelManagement.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class NewOrderRealTimeModel : PageModel
    {
        private readonly IOrderService _orderService;

        public NewOrderRealTimeModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var allNewOrder = await _orderService.GetOrderByStatus((int)(OrderStatus.WaitingAccept));
            var allNewOrderDto = allNewOrder.Select(order => new OrderDTO
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                OrderDate = order.OrderDate?.ToString("dd-MM-yyyy"),
                Status = order.Status,
                Address = order.Address,
                UserId = order.UserId,
                PaymentName = order.Payment?.PaymentName,
                VoucherPercent = order.Voucher?.Percent,
                TotalProduct = order.TotalProduct,
                TotalPrice = order.TotalPrice?.ToVietnameseCurrency(),
            }).ToList();
            return new JsonResult(allNewOrderDto);
        }
    }
}
