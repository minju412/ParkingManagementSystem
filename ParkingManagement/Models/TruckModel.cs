﻿using ParkingManagement.Lib.DataBase;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class TruckModel : CarModel
    {
        public override int Insert()
        {
            string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag,car_type) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y','t')";

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

            return _calcFee(model, basic_min, basic_fee, add_min, add_fee);
        }
    }
}