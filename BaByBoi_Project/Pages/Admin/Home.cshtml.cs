using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.BusinessModel;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.Admin
{
    public class HomeModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public HomeModel(IOrderService orderService, IProductService productService, IUserService
             userService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
        }
        [BindProperty]
        public List<LineChartModels> LineChartModels { get; set; }
        [BindProperty]
        public List<BarChartModel> BarChartModels { get; set; }
        [BindProperty]
        public int NumberOfUser { get; set; }
        [BindProperty]
        public List<User> ListAllUser  { get; set; }
        [BindProperty]
        public List<Order> ListAllOrder { get; set; }
        [BindProperty]
        public double? Revenue { get; set; }
        [BindProperty]
        public int NumberOfOrder { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }

        public async Task OnGet()
        {
            LineChartModels = await _orderService.GetAllOrderByMonth();
            BarChartModels = await _productService.GetProductsForStatistic();

            NumberOfUser = await _userService.GetTotalUserAsync();

            Revenue = await _orderService.GetTotalRevenueAsync();

            NumberOfOrder = await _orderService.GetAllOrderCountAsync();

            var tempAllUser = await _userService.GetAll();
            ListAllUser = tempAllUser.OrderByDescending(x => x.CreateDate).Take(5).ToList();
            ListAllOrder = await _orderService.GetAllOrder();
            var tempUser = HttpContext.Session.GetObjectFromJson<User>("User");
            if(tempUser != null)
            {
                CurrentUser = tempUser;
            }
        }
    }
}
