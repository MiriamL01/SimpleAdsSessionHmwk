using Microsoft.AspNetCore.Mvc;
using SimpleAdsSessionHmwk.Web.Models;
using SimpleAdsSessionHmwk.Data;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SimpleAdsSessionHmwk.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Index()
        {
            var db = new SimpleAdDb(_connectionString);

            var vm = (new HomePageViewModel
            {
                Ads = db.GetAllSimpleAds(),
                Ids = HttpContext.Session.Get<List<int>>("UserAdIds") ?? new List<int>()
            });
            return View(vm);
        }
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(SimpleAd ad)
        {
            var db = new SimpleAdDb(_connectionString);
            db.AddSimpleAd(ad);
            var ids = HttpContext.Session.Get<List<int>>("UserAdIds") ?? new List<int>();
            ids.Add(ad.Id);
            HttpContext.Session.Set("UserAdIds", ids);
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            List<int> ids = HttpContext.Session.Get<List<int>>("UserAdIds");
            if (ids != null && ids.Contains(id))
            {
                var db = new SimpleAdDb(_connectionString);
                db.DeleteAd(id);
            }
            return Redirect("/");
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
