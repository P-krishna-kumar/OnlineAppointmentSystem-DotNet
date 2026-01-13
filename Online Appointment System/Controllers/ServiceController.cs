using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models;


namespace Online_Appointment_System.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServiceDAL _serviceDal;

        public ServiceController(ServiceDAL serviceDal)
        {
            _serviceDal = serviceDal;
        }

        public IActionResult Index()
        {
            var dt = _serviceDal.ListService();
            return View(dt);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Service model)
        {
            if (ModelState.IsValid)
            {
                _serviceDal.AddService(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var dt = _serviceDal.GetById(id);
            if (dt.Rows.Count == 0)
                return NotFound();

            Service model = new Service
            {
                ServiceId = id,
                ServiceName = dt.Rows[0]["ServiceName"].ToString(),
                Description = dt.Rows[0]["Description"].ToString(),
                Price = Convert.ToDecimal(dt.Rows[0]["Price"])
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Service model)
        {
            if (ModelState.IsValid)
            {
                _serviceDal.UpdateService(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _serviceDal.DeleteService(id);
            return RedirectToAction("Index");
        }

        public IActionResult ToggleStatus(int id)
        {
            _serviceDal.ToggleStatus(id);
            return RedirectToAction("Index");
        }

    }
}
