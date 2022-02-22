using ParkingManagement.Lib.DataBase;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class TruckModel : CarModel
    {
        public override int Insert()
        {
            string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag,car_type) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y','truck')";

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

            // 추가 60분당 4000원
            int add_min = 60;
            int add_fee = 4000;

            var model = new TruckModel();
            model = model.Get<TruckModel>(carnum);
            //var model = TruckModel.Get(carnum);

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