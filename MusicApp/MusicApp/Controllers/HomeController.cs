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
        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult Tabs()
        {
            return View();
        }


        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadSong(string title)
        {
            web.HttpPostedFileBase songFile = null;
            web.HttpPostedFileBase imageFile = null;
            if (Request.Files.Count > 0)
            {
                 songFile = Request.Files[0];

                if (songFile != null && songFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(songFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Songs"), fileName);
                    songFile.SaveAs(path);
                }

                imageFile = Request.Files[1];

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    imageFile.SaveAs(path2);
                }
            }
            Artist arty= neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Song song = new Song(arty, songFile, imageFile, title);
            neo.CreateSong(song, arty);

            return View();

        }

        public ActionResult Uploads()
        {
            return View();
        }
        


    }
}