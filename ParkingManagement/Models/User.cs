using Oracle.ManagedDataAccess.Client;

namespace ParkingManagement.Models
{
    public class User
    {
        public int User_Seq { get; set; }

        public string User_Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public void ConvertPassword() // password 암호화
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.Password.Length.ToString());

            // 암호화된 해쉬값
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Password));

            this.Password = System.Convert.ToBase64String(hash);
        }

        internal int Register() // 사용자 등록
        {
            string sql = "INSERT INTO c_user (user_seq,user_name,email,password) VALUES (C_USER_SEQ.NEXTVAL,:user_name,:email,:password)";

            string _strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=ann;Password=111111;";

            using (var conn = new OracleConnection(_strConn))
            {
                conn.Open();

                return Dapper.SqlMapper.Execute(conn, sql, this);
            }
        }
    }
}