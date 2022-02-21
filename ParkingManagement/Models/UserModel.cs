using Oracle.ManagedDataAccess.Client;
using ParkingManagement.Lib.DataBase;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class UserModel
    {
        public int User_Seq { get; set; }

        public string User_Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        // 관리자용 - 회원 리스트
        public static List<UserModel> GetUserList(string search)
        {
            string sql = "SELECT user_name,email FROM c_user WHERE role='user' ORDER BY user_seq ASC";

            using (var db = new DapperHelper())
            {
                return db.Query<UserModel>(sql, new { search = search });
            }
        }

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