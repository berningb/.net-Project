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

        public ActionResult ProfilePage()
        {
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/Images/"));
            Artist arty = neo.getArtist(artistName);
            List<Song> songs = neo.getSongs(arty, folder);
            List<Artist> Friends = neo.getFriends(arty);
            List<Artist> Following = neo.getFollowers(arty);

            Artist MainArty = new Artist(artistName, artistName, songs, Friends, Following);

            // ViewBag.Songs = neo.getSongs(arty);
            return View(MainArty);
        }

     
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult Search(string Input)
        {
            Artist arty = neo.getArtist(Input);
            if(arty != null)
            {
                return View("ProfilePage", arty);
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult UploadSong(string Title)
        {
          
            web.HttpPostedFileBase songFile = null;
            web.HttpPostedFileBase imageFile = null;
            string fileName = null;
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
                     fileName = Path.GetFileName(imageFile.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    imageFile.SaveAs(path2);
                }
            }
            
            
            Artist arty = neo.getArtist(artistName);
            Song song = null;
            song  = new Song(arty, Title, fileName);
            neo.CreateSong(song, arty);

            return View();

        }

        public ActionResult SongPage()
        {
            return View();
        }

    }
}