using BaByBoi.DataAccess.Service;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi.Domain.Repositories;
using BaByBoi.Domain.Utils;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.HistoryOrder
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
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
        public List<Order> ListHistoryOrderPagin { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }

        private async Task LoadDataAsync(string? PageIndex, string PageSize)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            var result = new List<Order>();
            var getAllOrder = await _orderService.GetAllOrder();
            if (user == null)
            {
                this.PageIndex = 1;
                this.PageSize = 5;
                result = await _orderService.GetAllOrder();

            } else
            {
                result = await _orderService.getAllOrderOfUser(user.UserId);
            }
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
                else if (int.Parse(PageIndex) > (int)Math.Ceiling(getAllOrder.Count / (double)int.Parse(PageSize)))
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
                TotalPage = (int)Math.Ceiling(getAllOrder.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
            catch (Exception)
            {
                this.PageIndex = PageHelper.DefaultPageIndex;
                this.PageSize = PageHelper.DefaultPageSize;
                ListHistoryOrderPagin = PaginatedList<Order>.Create(
                       result.AsQueryable(), this.PageIndex, this.PageSize);
                TotalPage = (int)Math.Ceiling(getAllOrder.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
        }

        public async Task OnGet(string PageIndex, string PageSize)
        {
            await LoadDataAsync(PageIndex, PageSize);
        }
        

    }
}
