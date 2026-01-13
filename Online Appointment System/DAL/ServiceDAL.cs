using Microsoft.Data.SqlClient;
using Online_Appointment_System.Models;
using System.Data;

namespace Online_Appointment_System.DAL
{
    public class ServiceDAL
    {

         
            private readonly string _conn;

            public ServiceDAL(IConfiguration config)
            {
                _conn = config.GetConnectionString("DefaultConnection");
            }

            // ADD Service
            public int AddService(Service model)
            {
                using (SqlConnection con = new SqlConnection(_conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_Service_Add", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ServiceName", model.ServiceName);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.Parameters.AddWithValue("@Price", model.Price);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }

            // List Service
            public DataTable ListService()
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(_conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_Service_List", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                return dt;
            }


        public DataTable GetById(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Service_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ServiceId", id);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void UpdateService(Service model)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Service_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ServiceId", model.ServiceId);
                cmd.Parameters.AddWithValue("@ServiceName", model.ServiceName);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@Price", model.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteService(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Service_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ToggleStatus(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Service_ToggleStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



    }
}
