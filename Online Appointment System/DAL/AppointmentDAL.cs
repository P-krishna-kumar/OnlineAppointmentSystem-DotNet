using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Online_Appointment_System.DAL
{
    public class AppointmentDAL
    {
        private readonly string _conn;

        public AppointmentDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public int AddAppointment(Appointment model)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Appointment_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@ServiceId", model.ServiceId);
                cmd.Parameters.AddWithValue("@TimeSlotId", model.TimeSlotId);
                cmd.Parameters.AddWithValue("@AppointmentDate", model.AppointmentDate);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetUserAppointments(int userId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Appointment WHERE UserId=@UserId", con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable AdminAppointments()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Appointment_List", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public int UpdateStatus(int appointmentId, string status)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Appointment_UpdateStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                cmd.Parameters.AddWithValue("@Status", status);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public string GetUserEmailByAppointment(int appointmentId)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT U.Email 
            FROM Appointment A
            JOIN Users U ON A.UserId = U.UserId
            WHERE AppointmentId=@AppointmentId
        ", con);

                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                con.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        internal void SendEmail(string email, string v, string body)
        {
            throw new NotImplementedException();
        }

        public DataTable GetAppointmentById(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT A.*, U.FullName, U.Email, 
                   S.ServiceName,
                   T.SlotFrom, T.SlotTo
            FROM Appointment A
            JOIN Users U ON A.UserId = U.UserId
            JOIN Service S ON A.ServiceId = S.ServiceId
            JOIN TimeSlot T ON A.TimeSlotId = T.TimeSlotId
            WHERE AppointmentId=@id
        ", con);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable AdminAppointmentsListForCalendar()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT A.AppointmentId, A.AppointmentDate, A.Status,
                   U.FullName AS UserName,
                   S.ServiceName
            FROM Appointment A
            JOIN Users U ON A.UserId = U.UserId
            JOIN Service S ON A.ServiceId = S.ServiceId
        ", con);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public int AddAppointmentWithPayment(Appointment model, string paymentId)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Appointment 
            (UserId, ServiceId, TimeSlotId, AppointmentDate, PaymentId, PaymentStatus)
            OUTPUT INSERTED.AppointmentId
            VALUES (@UserId, @ServiceId, @TimeSlotId, @AppointmentDate, @PaymentId, 'Paid')
        ", con);

                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@ServiceId", model.ServiceId);
                cmd.Parameters.AddWithValue("@TimeSlotId", model.TimeSlotId);
                cmd.Parameters.AddWithValue("@AppointmentDate", model.AppointmentDate);
                cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

    }
}
