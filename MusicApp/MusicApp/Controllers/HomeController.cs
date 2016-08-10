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
        string artistName = web.HttpContext.Current.User.Identity.Name;
      
        NeoMain neo = new NeoMain();
        public ActionResult Index()
        {
    
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

        public ActionResult ProfilePage()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadSong(string Title)
        {
          
            web.HttpPostedFileBase songFile = null;
            web.HttpPostedFileBase imageFile = null;
            if (Request.Files.Count > 0)
            {
                 songFile = Request.Files[0];

                //if (songFile != null && songFile.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(songFile.FileName);
                //    var path = Path.Combine(Server.MapPath("~/App_Data/Songs"), fileName);
                //    songFile.SaveAs(path);
                //}

                imageFile = Request.Files[1];

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    imageFile.SaveAs(path2);
                }
            }
            
            
            Artist arty = neo.getArtist(artistName);
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/Images"));
            string[] filesEntries = Directory.GetFiles(folder);

            byte[] imageFileBytes = null;
            string filename = null;
            foreach (var item in filesEntries)
            {
                filename = Path.GetFileName(item);
                imageFileBytes = System.IO.File.ReadAllBytes(item);
            }

            Song song = new Song(arty, imageFileBytes, Title, filename );
            neo.CreateSong(song, arty);
            return View();

        }
      
       

        public ActionResult Uploads()
        {
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/Images"));
            Artist arty = neo.getArtist(artistName);
            List<Song> songs = neo.getSongs(arty, folder);
           // ViewBag.Songs = neo.getSongs(arty);
            return View(neo.getSongs(arty, folder));
        }

        public ActionResult SongPage()
        {
            return View();
        }
        


    }
}