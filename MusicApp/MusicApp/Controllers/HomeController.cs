using System;
using System.Collections.Generic;
using System.Linq;
using web = System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;
using System.IO;
using Neo4j;
using MusicApp.Common;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        string artistName = web.HttpContext.Current.User.Identity.Name;
        CloudStorageAccount blobAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobConnectionString"));
        

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

            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");
            CloudBlockBlob blockBlob;

            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/MP3/"));
            Artist arty = neo.getArtist(artistName);
            List<Song> songs = neo.getSongs(arty, folder);
            foreach(Song s in songs)
            {
                blockBlob = container.GetBlockBlobReference(s.Title + "_song.mp3");
                using (var fileStream = System.IO.File.OpenWrite(folder + "/" + s.Title + "_song.mp3"))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
            }
            List<Artist> Friends = neo.getFriends(arty);
            List<Artist> Following = neo.getFollowers(arty);

            Artist MainArty = new Artist(artistName, artistName, songs, Friends, Following);

            // ViewBag.Songs = neo.getSongs(arty);
            return View(MainArty);
        }
        //public JsonResult returnArtist()
        //{
        //    return Json(neo.getArtist())
        //}


        public ActionResult Upload()
        {
            return View();
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
            //string imageFileName = null;
            //string songFileName = null;
            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");

            if (Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];
                songFile = Request.Files[1];
                
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    //imageFileName = Path.GetFileName(imageFile.FileName);
                    //var path = Path.Combine(Server.MapPath("~/Content/Images/"), imageFileName);
                    //imageFile.SaveAs(path);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Title + "_img.jpg");
                    using (var fileStream = imageFile.InputStream)
                    {
                        blockBlob.UploadFromStream(fileStream);
                    }
                }

                if (songFile != null && songFile.ContentLength > 0)
                {
                    //songFileName = Path.GetFileName(songFile.FileName);
                    //var path2 = Path.Combine(Server.MapPath("~/Content/MP3/"), songFileName);
                    //songFile.SaveAs(path2);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Title + "_song.mp3");
                    using (var fileStream = songFile.InputStream)
                    {
                        blockBlob.UploadFromStream(fileStream);
                    }
                }
            }
            Artist arty = neo.getArtist(artistName);
            Song song = new Song(arty, Title);//, imageFileName, songFileName);
            neo.CreateSong(song, arty);

            return RedirectToAction("ProfilePage");
        }

        public ActionResult SongPage()
        {
            return View();
        }
    }
}