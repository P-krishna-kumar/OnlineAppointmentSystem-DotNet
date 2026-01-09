using System.Data;
using Microsoft.Data.SqlClient;
using Online_Appointment_System.Models;

namespace Online_Appointment_System.DAL
{
    public class AdminDAL
    {

        private readonly string _conn;

        public AdminDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public Admin AdminLogin(string username, string password)
        {
            Admin admin = null;

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    admin = new Admin
                    {
                        AdminId = Convert.ToInt32(dr["AdminId"]),
                        Username = dr["Username"].ToString()
                    };
                }
            }
            return admin;
        }

    }
}
