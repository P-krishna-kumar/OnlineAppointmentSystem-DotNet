using Microsoft.Data.SqlClient;
using Online_Appointment_System.Models;
using System.Data;

namespace Online_Appointment_System.DAL
{
    public class UserDAL
    {

        private readonly string _conn;
        private readonly string _conStr;

         

        public UserDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
            _conStr = config.GetConnectionString("DefaultConnection");
        }

        // Register User
        public int Register(User model)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Register", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // User Login
        public User Login(string email, string password)
        {
            User user = null;

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(dr["UserId"]),
                        FullName = dr["FullName"].ToString(),
                        Email = dr["Email"].ToString()
                    };
                }
            }
            return user;
        }  

        public DataTable UserDashboardStats(int userId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Dashboard_Stats", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_List", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void BlockUser(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Block", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UnblockUser(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Unblock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("sp_User_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        

        // Get Profile
        public User GetUserById(int id)
        {
            User user = new User();

            using (SqlConnection con = new SqlConnection(_conStr))
            {
                string q = "SELECT * FROM Users WHERE UserId=@Id";

                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    user.UserId = Convert.ToInt32(dr["UserId"]);
                    user.FullName = dr["FullName"].ToString();
                    user.Email = dr["Email"].ToString();
                }

                con.Close();
            }

            return user;
        }

        // Update Profile
        public bool UpdateUser(User u)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                string q = "UPDATE Users SET FullName=@Name WHERE UserId=@Id";

                SqlCommand cmd = new SqlCommand(q, con);

                cmd.Parameters.AddWithValue("@Name", u.FullName);
                cmd.Parameters.AddWithValue("@Id", u.UserId);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i > 0;
            }
        }
    }
}
