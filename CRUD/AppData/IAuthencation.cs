using Azure.Identity;
using CRUD.Models;
using Microsoft.Data.SqlClient;

namespace CRUD.AppData
{
    public interface IAuthencation
    {
        bool Authenticate(User user);
    }
    public class Authentication : IAuthencation
    {
        private readonly string _ConStr;
        public Authentication(ConnectionHelper conn)
        {
            _ConStr = conn.Default;  
        }
        public bool Authenticate(User user)
        {
            using (SqlConnection con = new SqlConnection(_ConStr))
            {
                string sqlquery = "Select Count(*) from Users where Username=@Username and Password=@Password";
                SqlCommand cmd = new SqlCommand(sqlquery, con);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                con.Open();
                var result = cmd.ExecuteScalar();
                if (result == null || Convert.ToInt32(result) == 0)
                {
                    return false;
                }
                return Convert.ToInt32(result) > 0;
            }
        }
    }
}
