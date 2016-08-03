using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.List = NeoQuery();


            return View();
        }

        public ActionResult Wall()
        {
            return View();
        }

        public ActionResult Playlist()
        {
            return View();
        }


      
    }
}