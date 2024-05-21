using BaByBoi.DataAccess.Service;
using BaByBoi.DataAccess.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, UserService service)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }
    }
}
