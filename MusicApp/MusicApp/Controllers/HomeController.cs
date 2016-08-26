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
        bool picUploaded = false;

        public ActionResult Index()
        {
            List<Song> songs = neo.getAllSongs();

            return View(songs);
        }

        public ActionResult UnknownUser()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult Playlist()
        {

            return View();
        }


        public ActionResult ProfilePage()
        {

            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");
            CloudBlockBlob blockBlob;

            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/MP3/"));
            string imgfolder = Path.GetDirectoryName(Server.MapPath("~/Content/Images/"));
            Artist arty = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            List<Song> songs = neo.getSongs(arty);
            foreach (Song s in songs)
            {
                if (!System.IO.File.Exists(folder + "/" + s.Title + "_song.mp3"))
                {
                    blockBlob = container.GetBlockBlobReference(s.Title + "_song.mp3");
                    using (var fileStream = System.IO.File.OpenWrite(folder + "/" + s.Title + "_song.mp3"))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }
                if (!System.IO.File.Exists(imgfolder + "/" + s.ImageFileName))
                {
                    blockBlob = container.GetBlockBlobReference(s.ImageFileName);
                    using (var fileStream = System.IO.File.OpenWrite(imgfolder + "/" + s.ImageFileName))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }

            }
            if (picUploaded)
            {
                if (!System.IO.File.Exists(imgfolder + "/" + arty.ProfilePicture))
                {
                    blockBlob = container.GetBlockBlobReference(arty.ProfilePicture);
                    using (var fileStream = System.IO.File.OpenWrite(imgfolder + "/" + arty.ProfilePicture))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }
            }


            Artist MainArty = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            MainArty.ProfilePicture = neo.GetProfilePicture(MainArty);

            ViewBag.username = artistName;
            ViewBag.show = MyJsonConverter.Serialize(MainArty);


            return View(MainArty);
        }

        [HttpPost]
        public ActionResult EditProfile(string Title)
        {
            web.HttpPostedFileBase imageFile = null;
            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");

            string fileName = null;
            if (Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];

                if (imageFile != null && imageFile.ContentLength > 0)
                {

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Title + "_img.jpg");
                    fileName = Title + "_img.jpg";
                    using (var fileStream = imageFile.InputStream)
                    {
                        picUploaded = true;
                        blockBlob.UploadFromStream(fileStream);
                    }
                }
            }
            Artist artist = neo.getArtist(artistName);
            artist.ProfilePicture = fileName;
            neo.AddProfilePicture(artist);
            return RedirectToAction("ProfilePage");
        }


        public void Follow(string name)
        {

            Artist curr = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Artist follow = neo.getArtist(name);
            neo.FollowArtist(curr, follow);

            Response.Redirect(Request.RawUrl);
        }

        public void Unfollow(string name)
        {

            Artist curr = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Artist follow = neo.getArtist(name);
            neo.Unfollow(curr, follow);

            Response.Redirect(Request.RawUrl);

        }

        public ActionResult Search(string Input)
        {
            Artist arty = neo.getArtist(Input);
            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");
            CloudBlockBlob blockBlob;
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/MP3/"));
            string imgfolder = Path.GetDirectoryName(Server.MapPath("~/Content/Images/"));
            if (arty != null)
            {
                List<Song> songs = neo.getSongs(arty);
                foreach (Song s in songs)
                {
                    if (!System.IO.File.Exists(folder + "/" + s.Title + "_song.mp3"))
                    {
                        blockBlob = container.GetBlockBlobReference(s.Title + "_song.mp3");
                        using (var fileStream = System.IO.File.OpenWrite(folder + "/" + s.Title + "_song.mp3"))
                        {
                            blockBlob.DownloadToStream(fileStream);
                        }
                    }
                    if (!System.IO.File.Exists(folder + "/" + s.Title + "_img.jpg"))
                    {
                        blockBlob = container.GetBlockBlobReference(s.Title + "_img.jpg");
                        using (var fileStream = System.IO.File.OpenWrite(imgfolder + "/" + s.Title + "_img.jpg"))
                        {
                            blockBlob.DownloadToStream(fileStream);
                        }
                    }
                }

                Artist arty3 = neo.getArtist(Input);
                String newId = arty3.Id;
                Artist finalArtist = new Artist(newId, arty.Name, arty.Email, songs, neo.getFriends(arty), neo.getPeopleYouFollow(arty), neo.getFollowers(arty));
                ViewBag.me = web.HttpContext.Current.User.Identity.Name;
                ViewBag.show = MyJsonConverter.Serialize(finalArtist);

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
            string imageFileName = null;

            if (Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];
                songFile = Request.Files[1];

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Title + "_img.jpg");
                    imageFileName = Title + "_img.jpg";
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
            song.ImageFileName = imageFileName;
            neo.CreateSong(song, arty);

            return RedirectToAction("ProfilePage");
        }

        public ActionResult Song(string id)
        {
            ViewBag.id = id;
            List<Song> songs = neo.getAllSongs();

            return View(songs);
        }
    }
}