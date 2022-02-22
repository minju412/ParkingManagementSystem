using PagedList;
using ParkingManagement.Models;
using System.Web.Mvc;

namespace ParkingManagement.Controllers
{
    public class AdminController : Controller
    {
        // 입출차 기록 확인
        public ActionResult TotalTableList(int? page, string search)
        {
            int pageNum = (page ?? 1);
            int listCount = 5;

            var cars = CarModel.GetTotalList(search);

            return View(cars.ToPagedList(pageNum, listCount));
        }

        // 주차장 수입 확인
        public ActionResult CheckFee(string search)
        {
            return View(CarModel.GetTotalList(search));
        }

        // 회원 확인
        public ActionResult MemberList(int? page, string search)
        {
            int pageNum = (page ?? 1);
            int listCount = 5;

            var users = UserModel.GetUserList(search);
            
            return View(users.ToPagedList(pageNum, listCount));
        }

    }
}