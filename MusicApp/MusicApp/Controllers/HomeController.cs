using System;
using System.Collections.Generic;
using System.Linq;
using web = System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;
using System.IO;
using Neo4j;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {

        NeoMain neo = new NeoMain();
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

        public ActionResult Following()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]


        public ActionResult UploadSong()
        {
            web.HttpPostedFileBase file = null;
            if (Request.Files.Count > 0)
            {
                 file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    file.SaveAs(path);
                }
            }
            Artist arty= neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Song song = new Song(arty, file);
            neo.CreateSong(song, arty);

            return View();

        }

        public ActionResult Overview()
        {
            return View();
        }



    }
}