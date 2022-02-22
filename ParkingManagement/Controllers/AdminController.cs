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

            var cars = CarModel.GetTotalList(search);

            return View(cars.ToPagedList(pageNum, listCount));
        }

        public ActionResult CheckFee(string search)
        {
            return View(CarModel.GetTotalList(search));
        }

        public ActionResult MemberList(int? page, string search)
        {
            int pageNum = (page ?? 1);
            int listCount = 5;

            var users = UserModel.GetUserList(search);

            return View(users.ToPagedList(pageNum, listCount));
        }
    }
}