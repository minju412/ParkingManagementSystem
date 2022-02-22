using ParkingManagement.Lib.DataBase;
using System;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public abstract class VehiclesModel
    {
        public int Car_ID { get; set; }

        public string CarNum { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }

        public string Owner_Name { get; set; }

        public int Parking_Fee { get; set; }

        public string Flag { get; set; }

        public string Car_Type { get; set; }

        // 사용자용 (주차장 이용 중인 차)
        public static List<VehiclesModel> GetList(string search)
        {
            string sql = "SELECT * FROM c_table WHERE flag='y' ORDER BY car_id ASC";

            using (var db = new DapperHelper())
            {
                return db.Query<VehiclesModel>(sql, new { search = search });
            }
        }


        // 관리자용 (주차장 이용 끝난 차)
        public static List<VehiclesModel> GetTotalList(string search)
        {
            string sql = "SELECT * FROM c_table WHERE flag='n' ORDER BY car_id ASC";

            using (var db = new DapperHelper())
            {
                return db.Query<VehiclesModel>(sql, new { search = search });
            }
        }

        public abstract VehiclesModel Get(string carnum);

        public abstract int Insert();


        //public static CarModel Get(string carnum)
        //{
        //    string sql = "SELECT * FROM c_table WHERE carnum=:carnum AND flag='y'";

        //    using (var db = new DapperHelper())
        //    {
        //        return db.QuerySingle<CarModel>(sql, new { carnum = carnum });
        //    }
        //}

        //public virtual int Insert() // virtual - 상속
        //{
        //    string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag,car_type) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y','car')";

        //    using (var db = new DapperHelper())
        //    {
        //        return db.Execute(sql, this);
        //    }
        //}


        public int UpdateOutTime() // 출차 시각 update
        {
            string sql = "UPDATE c_table SET outtime=SYSDATE WHERE carnum=:carnum";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int UpdateFee(int fee) // 주차 요금 update
        {
            string sql = "UPDATE c_table SET parking_fee=" + fee.ToString() + " WHERE carnum=:carnum AND flag='y'";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int Delete() // 출차
        {

            string sql = "UPDATE c_table SET flag='n' WHERE carnum=:carnum AND flag='y'";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        //public int CalcMin(string carnum)
        //{
        //    var model = CarModel.Get(carnum);
        //    int min = (model.OutTime.Day * 24 * 60 + model.OutTime.Hour * 60 + model.OutTime.Minute) - (model.InTime.Day * 24 * 60 + model.InTime.Hour * 60 + model.InTime.Minute);

        //    return min;
        //}

        public abstract int CalcFee(string carnum);
       

    }
}