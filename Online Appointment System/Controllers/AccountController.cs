using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.Models;
 

namespace Online_Appointment_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
        }

        // ================= LOGIN =================
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT UserID, Name, Role 
                      FROM Users 
                      WHERE Email=@Email AND Password=@Password", con);

                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Password", model.Password);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    HttpContext.Session.SetInt32("UserId", (int)dr["UserID"]);
                    HttpContext.Session.SetString("UserName", dr["Name"].ToString());
                    HttpContext.Session.SetString("UserRole", dr["Role"].ToString());

                    // Role based redirect
                    if (dr["Role"].ToString() == "Admin")
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError("", "Invalid Email or Password");
            return View(model);
        }

        // ================= REGISTER =================
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (SqlConnection con = GetConnection())
            {
                con.Open();

                // 🔹 Check Email Exists
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Email=@Email", con);
                checkCmd.Parameters.AddWithValue("@Email", model.Email);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                // 🔹 Insert User
                SqlCommand insertCmd = new SqlCommand(
                    @"INSERT INTO Users (Name, Email, Password, Role)
                      VALUES (@Name, @Email, @Password, 'User')", con);

                insertCmd.Parameters.AddWithValue("@Name", model.Name);
                insertCmd.Parameters.AddWithValue("@Email", model.Email);
                insertCmd.Parameters.AddWithValue("@Password", model.Password);

                insertCmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Registration Successful! Login now.";
            return RedirectToAction("Login");
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

