using PagedList;
using ParkingManagement.Models;
using System.Web.Mvc;

namespace ParkingManagement.Controllers
{
    public class AdminController : Controller
    {
        // 관리자 - 주차장 수입 보기
        public ActionResult TotalTableList(int? page, string search)
        {
            int pageNum = (page ?? 1);
            int listCount = 5;

            var cars = Car.GetTotalList(search);

            return View(cars.ToPagedList(pageNum, listCount));
        }

        //public ActionResult TotalTableList(string search)
        //{
        //    return View(Car.GetTotalList(search));
        //}

        public ActionResult CheckFee(string search)
        {
            return View(Car.GetTotalList(search));
        }

        //int CalcTotalFee(string search)
        //{
        //    int tot_fee = 0;

        //    var cars = Car.GetTotalList(search);

        //    foreach (var item in cars)
        //    {
        //        tot_fee += item.Parking_Fee;
        //    }

        //    return tot_fee;
        //}

    }
}