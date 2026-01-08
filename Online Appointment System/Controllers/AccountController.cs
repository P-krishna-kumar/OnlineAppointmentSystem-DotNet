using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.Models;
 

namespace Online_Appointment_System.Controllers
{
    public class AccountController : Controller
    {
         
         
        public IActionResult Login()
        {
            return View();
        }

        
    }
}

