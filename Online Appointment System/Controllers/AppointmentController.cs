using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models;
using OnlineAppointmentSystem.Helpers;
using Razorpay.Api;
using static Online_Appointment_System.DAL.TimeSlatDAL;

namespace Online_Appointment_System.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly AppointmentDAL _appointmentDAL;
        private readonly ServiceDAL _serviceDAL;
        private readonly TimeSlotDAL _slotDAL;
        private readonly EmailHelper _email;
        public AppointmentController(AppointmentDAL appointmentDAL, ServiceDAL serviceDAL, TimeSlotDAL slotDAL)
        {
            _appointmentDAL = appointmentDAL;
            _serviceDAL = serviceDAL;
            _slotDAL = slotDAL;
        }

         //✅ ONLY ONE CONSTRUCTOR

        private readonly AppointmentDAL _appointmentDal;


        public AppointmentController(AppointmentDAL appointmentDal)
        {
            _appointmentDal = appointmentDal;
        }

        public AppointmentController(ServiceDAL serviceDAL)
        {
            _serviceDAL = serviceDAL;
        }

        [HttpGet]
        public IActionResult Create()
        { 
            ViewBag.Services = _serviceDAL.ListService();
            ViewBag.TimeSlots = _slotDAL.ListTimeSlot();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Appointment model)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Account");

            model.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            _appointmentDAL.AddAppointment(model);
            return RedirectToAction("MyAppointments");
        }

        public IActionResult MyAppointments()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var dt = _appointmentDAL.GetUserAppointments(userId);
            return View(dt);
        }

        //AJAX APPOINTMENT BOOKING(FULL SYSTEM)

          
        //public AppointmentController(AppointmentDAL appointmentDAL, ServiceDAL serviceDAL, TimeSlotDAL slotDAL, EmailHelper email)
        //{
        //    _appointmentDAL = appointmentDAL;
        //    _serviceDAL = serviceDAL;
        //    _slotDAL = slotDAL;
        //    _email = email;
        //}

        [HttpPost]
        public JsonResult AjaxCreate([FromBody] Appointment model)
        {
            try
            {
                if (HttpContext.Session.GetString("UserId") == null)
                {
                    return Json(new { status = false, message = "Not Logged In" });
                }

                model.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                _appointmentDAL.AddAppointment(model);

                // ------------ SEND EMAIL -------------
                string email = HttpContext.Session.GetString("UserEmail");

                string body = $@"
            <h3>Appointment Confirmation</h3>
            <p>Your appointment has been booked successfully!</p>
            <p><strong>Date:</strong> {model.AppointmentDate}</p>
            <p><strong>Service ID:</strong> {model.ServiceId}</p>
            <p><strong>Time Slot ID:</strong> {model.TimeSlotId}</p>
            <br/>
            <p>Thank you for using our service.</p>
        ";

                _email.SendEmail(email, "Appointment Confirmed", body);

                return Json(new { status = true, message = "Appointment Booked Successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        public IActionResult DownloadSlip(int id)
        {
            var dt = _appointmentDAL.GetAppointmentById(id);

            if (dt.Rows.Count == 0)
                return Content("Invalid Appointment");

            var row = dt.Rows[0];

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Title
                var title = new Paragraph("APPOINTMENT SLIP\n\n");
                title.Alignment = Element.ALIGN_CENTER;
                title.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                doc.Add(title);

                // Details Table
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;

                void AddRow(string label, string value)
                {
                    table.AddCell(new Phrase(label));
                    table.AddCell(new Phrase(value));
                }

                AddRow("Appointment ID", row["AppointmentId"].ToString());
                AddRow("User", row["FullName"].ToString());
                AddRow("Service", row["ServiceName"].ToString());
                AddRow("Date", row["AppointmentDate"].ToString());
                AddRow("Time Slot", row["SlotFrom"] + " - " + row["SlotTo"]);
                AddRow("Status", row["Status"].ToString());
                AddRow("Booking Date", row["BookingDate"].ToString());

                doc.Add(table);

                doc.Close();
                return File(ms.ToArray(), "application/pdf", "AppointmentSlip.pdf");
            }
        }

        [HttpPost]
        //public JsonResult CreateOrder([FromBody] Appointment model)
        //{
        //    try
        //    {
        //        var key = _config["Razorpay:Key"];
        //        var secret = _config["Razorpay:Secret"];

        //        RazorpayClient client = new RazorpayClient(key, secret);

        //        int amount = 500 * 100; // Rs 500 appointment fees

        //        Dictionary<string, object> options = new Dictionary<string, object>();
        //        options.Add("amount", amount);
        //        options.Add("currency", "INR");
        //        options.Add("payment_capture", 1);

        //        var order = client.Order.Create(options);

        //        return Json(new
        //        {
        //            orderId = order["id"].ToString(),
        //            amount = amount,
        //            key = key
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}
        [HttpPost]
        public JsonResult PaymentSuccess([FromBody] dynamic data)
        {
            try
            {
                string paymentId = data.paymentId;
                string orderId = data.orderId;

                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                Appointment model = new Appointment
                {
                    UserId = userId,
                    ServiceId = (int)data.serviceId,
                    TimeSlotId = (int)data.timeSlotId,
                    AppointmentDate = (string)data.date
                };

                int id = _appointmentDAL.AddAppointmentWithPayment(model, paymentId);

                return Json(new { status = true, message = "Appointment Confirmed!", appointmentId = id });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }


    }
}
