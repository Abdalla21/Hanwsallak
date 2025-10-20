using Hanwsallak.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    public class HomeController(ApplicationDBContext dbContext) : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
