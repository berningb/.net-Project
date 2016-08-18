using System;
using System.Collections.Generic;
using System.Linq;
using web = System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;
using System.IO;
using Neo4j;
using MusicApp.Common;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        string artistName = web.HttpContext.Current.User.Identity.Name;

        NeoMain neo = new NeoMain();




        Artist art = new Artist("bill", "fga@gail.com");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult SongPage()
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
            ViewBag.show = JsonConverter.Serialize(MainArty);

            return View(MainArty);
        }
       
        public ActionResult Search(string Input)
        {
            Artist arty = neo.getArtist(Input);
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/Images/"));
            if (arty != null)
            {
                Artist finalArtist = new Artist(arty.Name, arty.Email, neo.getSongs(arty, folder), neo.getFriends(arty), neo.getFollowers(arty));
                return View("ProfilePage", finalArtist);
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult UploadSong(string Title)
        {
            web.HttpPostedFileBase songFile = null;
            web.HttpPostedFileBase imageFile = null;
            string fileName = null;
            string fileName2 = null;
            if (Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];
                songFile = Request.Files[1];
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(imageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    imageFile.SaveAs(path);
                }
                if (songFile != null && songFile.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(songFile.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Content/MP3/"), fileName2);
                    songFile.SaveAs(path2);
                }
            }
            Artist arty = neo.getArtist(artistName);
            Song song = null;
            song = new Song(arty, Title, fileName, fileName2);
            neo.CreateSong(song, arty);
            return RedirectToAction("ProfilePage");
        }
    }
}