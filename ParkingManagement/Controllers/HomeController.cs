using PagedList;
using ParkingManagement.Models;
using System;
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

            var cars = Car.GetList(search);

            return View(cars.ToPagedList(pageNum, listCount));
        }

        [Authorize] // 로그인 해야 입차 가능
        public ActionResult TableInsert()
        {
            return View();
        }

        [Authorize]
        public ActionResult TableInsert_Input(string carnum)
        {
            var model = new Car();

            model.CarNum = carnum;
            //model.Owner = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.Owner_Name = User.Identity.Name;

            model.Insert();

            return Content("<script> alert('차량번호: " + model.CarNum + " 입차합니다.'); location.href = '/home/TableList'; </script>");
        }

        [Authorize]
        public ActionResult TableDelete()
        {
            return View();
        }

        // 차량 번호는 Input에서 받음..
        [Authorize]
        public ActionResult TableDelete_Input(string carnum)
        {
            var model = Car.Get(carnum);

            // 권한 확인
            //var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //if (model.Owner != userSeq)
            //{
            //    throw new Exception("수정할 수 없습니다.");
            //}

            model.UpdateOutTime(); // db update - 출차시각 
            model = Car.Get(carnum); // 출차 시각 업데이트된 모델 받음

            int fee = CalcFee(carnum);

            model.UpdateFee(fee); // db update - 주차요금
            model = Car.Get(carnum); // 주차요금 업데이트된 모델 받음

            model.Delete(); // db delete

            // alert 후 redirect
            return Content("<script> alert('차량번호: " + model.CarNum + " 출차합니다.\\n주차 요금은" + model.Parking_Fee.ToString() + "원 입니다.'); location.href = '/home/TableList'; </script>");

            throw new Exception("잘못된 요청입니다");
        }

        // 주차 요금 계산
        int CalcFee(string carnum)
        {
            // 30분 미만 기본요금 3000원
            int basic_min = 30;
            int basic_fee = 3000;

            // 추가 10분당 1000원
            int add_min = 10;
            int add_fee = 1000;

            var model = Car.Get(carnum);

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

        public ActionResult ParkingView()
        {
            return View();
        }
    }
}