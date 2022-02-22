using ParkingManagement.Lib.DataBase;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class BusModel : CarModel
    {
        public override int Insert()
        {
            string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag,car_type) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y','b')";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public override int CalcFee(string carnum)
        {
            // 60분 미만 기본요금 4000원
            int basic_min = 60;
            int basic_fee = 4000;

            // 추가 30분당 2000원
            int add_min = 30;
            int add_fee = 2000;

            var model = new BusModel();
            model = model.Get<BusModel>(carnum);

            return _calcFee(model, basic_min, basic_fee, add_min, add_fee);
        }
    }
} 