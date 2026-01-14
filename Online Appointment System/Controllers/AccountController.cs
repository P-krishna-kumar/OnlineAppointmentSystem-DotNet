using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models; 

namespace Online_Appointment_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly AdminDAL _adminDal;
        private readonly UserDAL _userDal;

        public AccountController(AdminDAL adminDal, UserDAL userDal)
        {
            _adminDal = adminDal;
            _userDal = userDal;
        }

        

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string Username, string Password)
        {
            var admin = _adminDal.AdminLogin(Username, Password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
                HttpContext.Session.SetString("AdminName", admin.Username);

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Error = "Invalid Username or Password";
                return View();
            }
        }

        

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                _userDal.Register(model);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // --------------------- LOGIN ---------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var user = _userDal.Login(Email, Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.FullName);

                return RedirectToAction("Create", "Appointment");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }

        // LOGOUT
        public IActionResult Logout()
        {
            string role = HttpContext.Session.GetString("Role");

            HttpContext.Session.Clear();

            if (role == "Admin")
            {
                return RedirectToAction("AdminLogin");
            }

            return RedirectToAction("Login"); // normal user
        }


    }
}

