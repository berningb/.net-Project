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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Wall()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfileImage(string Title)
        {
            web.HttpPostedFileBase imageFile = null;
            string fileName = null;
            if (Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];


                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(imageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    imageFile.SaveAs(path);
                }

            }
            Artist arty = neo.getArtist(artistName);
            arty.ProfilePicture = fileName;
            neo.AddProfilePicture(arty);
            return RedirectToAction("ProfilePage");
        }


        public ActionResult ProfilePage()
        {
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/Images/"));
            Artist arty = neo.getArtist(artistName);
            List<Song> songs = neo.getSongs(arty, folder);
            List<Artist> Friends = neo.getFriends(arty);
            List<Artist> Following = neo.getFollowers(arty);

            Artist MainArty = new Artist(artistName, artistName, songs, Friends, Following);
            MainArty.ProfilePicture = neo.GetProfilePicture(MainArty);

            // ViewBag.Songs = neo.getSongs(arty);
            return View(MainArty);
        }

        [HttpPost]
        public ActionResult EditProfile(string Title)
        {
            web.HttpPostedFileBase imageFile = null;
            string fileName = null;
            if(Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];

                if(imageFile != null && imageFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(imageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    imageFile.SaveAs(path);
                }
            }
            Artist artist = neo.getArtist(artistName);
            neo.AddProfilePicture(artist);

            return RedirectToAction("ProfilePage");
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

        public ActionResult SongPage()
        {
            return View();
        }
    }
}