using Microsoft.AspNetCore.Mvc;

namespace proyectoDB2_condominios.Controllers
{
    //[Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        //[HttpGet("{menuId}/menuitems")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddUserForm()
        {
            return View();
        }
    }
}
