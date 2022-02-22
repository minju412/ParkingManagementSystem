using ParkingManagement.Lib.DataBase;
using System;
using System.Collections.Generic;

namespace ParkingManagement.Models
{
    public class CarModel
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
        public static List<CarModel> GetList(string search)
        {
            string sql = "SELECT * FROM c_table WHERE flag='y' ORDER BY car_id ASC"; 

            using (var db = new DapperHelper())
            {
                return db.Query<CarModel>(sql, new { search = search });
            }
        }

        // 관리자용 (주차장 이용 끝난 차)
        public static List<CarModel> GetTotalList(string search)
        {
            string sql = "SELECT * FROM c_table WHERE flag='n' ORDER BY car_id ASC"; 

            using (var db = new DapperHelper())
            {
                return db.Query<CarModel>(sql, new { search = search });
            }
        }

        public T Get<T>(string carnum) where T : CarModel
        {
            string sql = "SELECT * FROM c_table WHERE carnum=:carnum AND flag='y'";

            using (var db = new DapperHelper())
            {
                return db.QuerySingle<T>(sql, new { carnum = carnum });
            }
        }

        //public static CarModel Get(string carnum) 
        //{
        //    string sql = "SELECT * FROM c_table WHERE carnum=:carnum AND flag='y'";

        //    using (var db = new DapperHelper())
        //    {
        //        return db.QuerySingle<CarModel>(sql, new { carnum = carnum });
        //    }
        //}

        public virtual int Insert() // virtual - 상속
        {
            string sql = "INSERT INTO c_table (car_id,carnum,intime,owner_name,flag) VALUES (C_TABLE_SEQ.NEXTVAL,:carnum,SYSDATE,:owner_name,'y')";

            using (var db = new DapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

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

        // 주차 요금 계산
        public virtual int CalcFee(string carnum)
        {
            // 30분 미만 기본요금 1000원
            int basic_min = 30;
            int basic_fee = 1000;

            // 추가 10분당 500원
            int add_min = 10;
            int add_fee = 500;


            var model = new CarModel();
            model = model.Get<CarModel>(carnum);
            //var model = CarModel.Get(carnum);

            // 주차 시간 계산 (분으로 환산)
            int min = (model.OutTime.Day * 24 * 60 + model.OutTime.Hour * 60 + model.OutTime.Minute) - (model.InTime.Day * 24 * 60 + model.InTime.Hour * 60 + model.InTime.Minute);
            //int min = CalcMin(carnum);

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