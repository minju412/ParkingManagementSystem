using ParkingManagement.Lib.DataBase;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class BusModel : VehiclesModel
    {
        //public override List<BusModel> GetList(string search)
        //{
        //    string sql = "SELECT * FROM c_table WHERE flag='y' ORDER BY car_id ASC";

        //    using (var db = new DapperHelper())
        //    {
        //        return db.Query<BusModel>(sql, new { search = search });
        //    }
        //}

        //public override List<BusModel> GetTotalList(string search)
        //{
        //    string sql = "SELECT * FROM c_table WHERE flag='n' ORDER BY car_id ASC";

        //    using (var db = new DapperHelper())
        //    {
        //        return db.Query<BusModel>(sql, new { search = search });
        //    }
        //}

        public override VehiclesModel Get(string carnum)
        {
            string sql = "SELECT * FROM c_table WHERE carnum=:carnum AND flag='y'";

            using (var db = new DapperHelper())
            {
                return db.QuerySingle<BusModel>(sql, new { carnum = carnum });
            }
        }

        public override int Insert()
        {
            string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag,car_type) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y','bus')";

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
            model = model.Get(carnum) as BusModel;
            //var model = BusModel.Get(carnum);

            // 주차 시간 계산 (분으로 환산)
            int min = (model.OutTime.Day * 24 * 60 + model.OutTime.Hour * 60 + model.OutTime.Minute) - (model.InTime.Day * 24 * 60 + model.InTime.Hour * 60 + model.InTime.Minute);

            // 주차 요금 계산
            int fee = 0;
            if (min >= basic_min)
            {
                fee += basic_fee;
                min -= basic_min;

                if (min % add_min == 0)
                {
                    fee += add_fee * (min / add_min);
                }
                else
                {
                    fee += add_fee * (min / add_min) + add_fee;
                }
            }
            else
                fee = basic_fee;

            return fee;
        }
    }
}