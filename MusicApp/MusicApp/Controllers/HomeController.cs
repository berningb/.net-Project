using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;
using System.IO;
using Neo4j;

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

        public ActionResult Following()
        {
            return View();






























        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult UploadSong(string name, HttpPostedFile image, HttpPostedFile song )
        //{
        //  //  Song newSong = new Song();

        //    newSong.Title = name;
        //    //newSong.File = song;
        //    //newSong.Image = image;
        //    //newSong.SongFileName = song.FileName;
        //    //newSong.ImageFileName = image.FileName;
        //    //newSong.length = song.ContentLength;
        //    //newSong.Owner.Name = User.Identity.Name;
            
        //    return View(newSong);

        //}

        public ActionResult Overview()
        {
            return View();
        }



    }
}