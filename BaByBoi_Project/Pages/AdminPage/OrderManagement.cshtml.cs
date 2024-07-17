using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi.Domain.Utils;
using BaByBoi_Project.Common.Enum;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class OrderManagementModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderManagementModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public int CurrentIndex { get; set; }

        [BindProperty]
        public int PageIndex { get; set; } = 1;
        [BindProperty]
        public int PageSize { get; set; } = 5;
        [BindProperty]
        public int TotalPage { get; set; }
        [BindProperty]
        public string Status { get; set; } = "WaitingAccept";
        [BindProperty]

        public List<Order> ListHistoryOrderPagin { get; set; }

        public async Task OnGet(string PageIndex, string PageSize, string status)
        {
            this.Status = status;
            int statusEnum = status switch
            {
                "WaitingAccept" => (int)(OrderStatus.WaitingAccept),
                "IsCancel" => (int)(OrderStatus.IsCancel),
                "IsReject" => (int)(OrderStatus.IsReject),
                "IsConfirmed" => (int)(OrderStatus.IsConfirmed),
                _ => 0
            };

            await LoadDataAsync(PageIndex, PageSize, statusEnum);
        }

        private async Task LoadDataAsync(string? PageIndex, string PageSize, int status)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            var result = await _orderService.GetOrderByStatus(status);
            this.PageIndex = 1;
            this.PageSize = 5;
            try
            {
                if (PageIndex == null && PageSize == null)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) <= 0)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) > (int)Math.Ceiling(result.Count / (double)int.Parse(PageSize)))
                {
                    this.PageIndex = TotalPage;
                    this.PageSize = int.Parse(PageSize);
                }
                else
                {
                    this.PageIndex = int.Parse(PageIndex);
                    this.PageSize = int.Parse(PageSize);
                }
                ListHistoryOrderPagin = PaginatedList<Order>.Create(
                        result.AsQueryable(), this.PageIndex, this.PageSize);
                TotalPage = (int)Math.Ceiling(result.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
            catch (Exception)
            {
                this.PageIndex = PageHelper.DefaultPageIndex;
                this.PageSize = PageHelper.DefaultPageSize;
                ListHistoryOrderPagin = PaginatedList<Order>.Create(
                       result.AsQueryable(), this.PageIndex, this.PageSize);
                TotalPage = (int)Math.Ceiling(result.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
        }

       
    }
}
