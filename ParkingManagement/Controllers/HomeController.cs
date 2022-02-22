using PagedList;
using ParkingManagement.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ParkingManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TableList(int? page, string search)
        {
            // Paging

            int pageNum = (page ?? 1);
            int listCount = 5;

            var cars = CarModel.GetList(search);

            return View(cars.ToPagedList(pageNum, listCount));
        }

        [Authorize] // 로그인 해야 입차 가능
        public ActionResult TableInsert(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [Authorize]
        public ActionResult TableInsert_Input(string carnum, string cartype)
        {
            try
            {
                VehiclesModel model;

                switch (cartype)
                {
                    case "truck":
                        model = new TruckModel();
                        break;

                    case "bus":
                        model = new BusModel();
                        break;

                    default:
                        model = new CarModel();
                        break;
                }

                model.CarNum = carnum;
                model.Owner_Name = User.Identity.Name;

                var cars = CarModel.GetList(carnum);
                foreach (var item in cars)
                {
                    if (model.CarNum == item.CarNum)
                    {
                        throw new Exception("이미 입차된 차량입니다");
                    }
                }

                model.Insert();

                return Content("<script> alert('차량번호: " + model.CarNum + " 입차합니다.'); location.href = '/home/TableList'; </script>");
            }
            catch (Exception ex)
            {
                // 실패
                return Redirect($"/home/tableinsert?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }

        [Authorize] // 로그인 해야 출차 가능
        public ActionResult TableDelete(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [Authorize]
        public ActionResult TableDelete_Input(string carnum)
        {
            try
            {
                VehiclesModel model = new CarModel();
                model = model.Get(carnum); // 문제: CarModel 안에서만 Get 해옴..

                if (model == null)
                {
                    throw new Exception("입차되지 않은 차량입니다");
                }
                if (model.Owner_Name != User.Identity.Name)
                {
                    throw new Exception("출차 권한이 없습니다.");
                }

                model.UpdateOutTime(); 

                int fee = model.CalcFee(carnum);

                model.UpdateFee(fee); 
                //model = CarModel.Get(carnum); // 주차요금 업데이트된 모델 받음 - 나중에 살리기

                model.Delete();

                return Content("<script> alert('차량번호: " + model.CarNum + " 출차합니다.\\n주차 요금은" + model.Parking_Fee.ToString() + "원 입니다.'); location.href = '/home/TableList'; </script>");
            }
            catch (Exception ex)
            {
                return Redirect($"/home/tabledelete?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }


        //[Authorize]
        //public ActionResult TableDelete_Input(string carnum)
        //{
        //    try
        //    {
        //        var model = CarModel.Get(carnum);

        //        if (model == null)
        //        {
        //            throw new Exception("입차되지 않은 차량입니다");
        //        }

        //        // 권한 확인
        //        if (model.Owner_Name != User.Identity.Name)
        //        {
        //            throw new Exception("출차 권한이 없습니다.");
        //        }

        //        model.UpdateOutTime(); // db update - 출차시각 
        //        //model = CarModel.Get(carnum); // 출차 시각 업데이트된 모델 받음 // 없어도 되는 듯..

        //        int fee = model.CalcFee(carnum);

        //        model.UpdateFee(fee); // db update - 주차요금
        //        model = CarModel.Get(carnum); // 주차요금 업데이트된 모델 받음

        //        model.Delete(); // db delete

        //        // alert 후 redirect
        //        return Content("<script> alert('차량번호: " + model.CarNum + " 출차합니다.\\n주차 요금은" + model.Parking_Fee.ToString() + "원 입니다.'); location.href = '/home/TableList'; </script>");
        //        //return Content("<script> alert('차량번호: " + model.CarNum + " 출차합니다.\\n주차 시간은" + model.CalcMin(carnum).ToString() + "분 입니다.\\n주차 요금은" + model.Parking_Fee.ToString() + "원 입니다.'); location.href = '/home/TableList'; </script>");
        //    }
        //    catch (Exception ex)
        //    {
        //        // 실패
        //        return Redirect($"/home/tabledelete?msg={HttpUtility.UrlEncode(ex.Message)}");
        //    }
        //}

        // 주차장 시설 확인
        public ActionResult ParkingView()
        {
            return View();
        }

    }
}