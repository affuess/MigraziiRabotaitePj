using Microsoft.AspNetCore.Mvc;

namespace MigraziiRabotaitePj.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
