using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Microsoft.Data.SqlClient;
using Online_Appointment_System.Models; 
using System.Data;

namespace Online_Appointment_System.DAL
{
    public class TimeSlatDAL
    {
        public class TimeSlotDAL
        {
            private readonly string _conn;

            public TimeSlotDAL(IConfiguration config)
            {
                _conn = config.GetConnectionString("DefaultConnection");
            }

            // Add Time Slot
            public int AddTimeSlot(TimeSlot model)
            {
                using (SqlConnection con = new SqlConnection(_conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_TimeSlot_Add", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SlotFrom", model.SlotFrom);
                    cmd.Parameters.AddWithValue("@SlotTo", model.SlotTo);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }

            // List TimeSlot
            public DataTable ListTimeSlot()
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(_conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_TimeSlot_List", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }

                return dt;
            }
        }


    }
}
