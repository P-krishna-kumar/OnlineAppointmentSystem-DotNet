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
    }
}
