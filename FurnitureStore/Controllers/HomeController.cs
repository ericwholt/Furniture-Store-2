using FurnitureStore.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace FurnitureStore.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakeNewUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User u)
        {
            User user = db.Users.Where(usr => usr.userName == u.userName && usr.password == u.password).ToList().First();

            Session["User"] = user;
            return RedirectToAction("Shop");
        }

        public ActionResult Shop()
        {
            List<Item> itemList = db.Items.ToList();
            return View(itemList);
        }

        [HttpPost]
        public ActionResult BuyItem(Item i)
        {
            decimal cost = decimal.Parse(i.price.ToString());
            User user = (User)Session["User"];
            if (user.funds >= cost)
            {
                User usr = db.Users.SingleOrDefault(u => u.id == user.id);
                Item item = db.Items.SingleOrDefault(x => x.id == i.id);
                decimal difference = decimal.Parse(usr.funds.ToString()) - cost;
                usr.funds = difference;
                item.quantity -= 1;
                UserItem ui = new UserItem() { ItemID=item.id, UserID=usr.id };
                db.UserItems.Add(ui);
                db.Users.AddOrUpdate(usr);
                db.Items.AddOrUpdate(item);
                db.SaveChanges();
                Session["User"] = usr;
            }
            else
            {
                Session["UserFunds"] = user.funds;
                Session["ItemCost"] = i.price;
                return RedirectToAction("ErrorPage");
                //Redirect to error page
            }


            return RedirectToAction("Shop");
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

    }

}