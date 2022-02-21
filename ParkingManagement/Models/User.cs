using Oracle.ManagedDataAccess.Client;
using ParkingManagement.Lib.DataBase;

namespace ParkingManagement.Models
{
    public class User
    {
        public int User_Seq { get; set; }

        public string User_Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public void ConvertPassword() // password 암호화
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.Password.Length.ToString());

            // 암호화된 해쉬값
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Password));

            this.Password = System.Convert.ToBase64String(hash);
        }

        internal int User_Register() // 사용자 등록
        {
            string sql = "INSERT INTO c_user (user_seq,user_name,email,password,role) VALUES (C_USER_SEQ.NEXTVAL,:user_name,:email,:password,'user')";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        internal int Admin_Register() // 관리자 등록
        {
            string sql = "INSERT INTO c_user (user_seq,user_name,email,password,role) VALUES (C_USER_SEQ.NEXTVAL,:user_name,:email,:password,'admin')";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }
    }
}