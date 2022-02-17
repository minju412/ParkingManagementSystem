using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkingManagement.ViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.Password.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Password));

            this.Password = System.Convert.ToBase64String(hash);
        }

        internal LoginViewModel GetLoginUser()
        {
            // this.User_Name
            // this.Password

            string sql = "SELECT user_seq,user_name,email,password FROM c_user WHERE email=:email";

            LoginViewModel user;

            string _strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=ann;Password=111111;";

            using (var conn = new OracleConnection(_strConn))
            {
                conn.Open();

                user = Dapper.SqlMapper.QuerySingleOrDefault<LoginViewModel>(conn, sql, this);
            }

            if (user == null)
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            if (user.Password != this.Password)
            {
                throw new Exception("비밀번호가 틀립니다.");
                // 틀린 횟수, ...
            }

            return user;
        }
    }
}