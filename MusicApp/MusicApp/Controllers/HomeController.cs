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
        List<Song> songs = neo.getAllSongs();

            return View(songs);
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
        public void FollowUser()
        {
            Artist arty = neo.getArtist(artistName);
           // neo.FollowArtist(arty, )
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
            Artist arty = neo.getArtist(artistName);
            List<Song> songs = neo.getSongs(arty);
            foreach(Song s in songs)
            {
                if(!System.IO.File.Exists(folder + "/" + s.Title + "_song.mp3"))
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
                    using(var fileStream = System.IO.File.OpenWrite(imgfolder + "/" + s.ImageFileName))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }
                if(!System.IO.File.Exists(imgfolder + "/" + arty.ProfilePicture))
                {
                    blockBlob = container.GetBlockBlobReference(arty.ProfilePicture);
                    using(var fileStream = System.IO.File.OpenWrite(imgfolder + "/" + arty.ProfilePicture))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }

            }


            List<Artist> Friends = neo.getFriends(arty);
            List<Artist> followers = neo.getFollowers(arty);
            List<Artist> following = neo.getPeopleYouFollow(arty);

            Artist MainArty = new Artist(artistName, artistName, songs, Friends, following, followers);
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
            if(Request.Files.Count > 0)
            {
                imageFile = Request.Files[0];

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                   
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Title + "_img.jpg");
                    fileName = Title + "_img.jpg";
                    using (var fileStream = imageFile.InputStream)
                    {
                        blockBlob.UploadFromStream(fileStream);
                    }
                }
            }
            Artist artist = neo.getArtist(artistName);
            artist.ProfilePicture = fileName;
            neo.AddProfilePicture(artist);          
            return RedirectToAction("ProfilePage");
        }


        public ActionResult Follow(string name)
        {
            
            Artist curr = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Artist follow = neo.getArtist(name);
            neo.FollowArtist(curr, follow);
            return View("ProfilePage", curr);   
        }

        public ActionResult UnFollow(string name)
        {

            Artist curr = neo.getArtist(web.HttpContext.Current.User.Identity.Name);
            Artist follow = neo.getArtist(name);
            neo.Unfollow(curr, follow);
            return View("ProfilePage", curr);
        }

        public ActionResult Search(string Input)
        {
            Artist arty = neo.getArtist(Input);
            CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container");
            CloudBlockBlob blockBlob;
            string folder = Path.GetDirectoryName(Server.MapPath("~/Content/MP3/"));
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
                }
                bool isFollowing = false;
                Artist finalArtist = new Artist(arty.Name, arty.Email, songs, neo.getFriends(arty),neo.getPeopleYouFollow(arty), neo.getFollowers(arty));
                foreach(var item in finalArtist.Followers)
                {
                    if(item.Name == artistName)
                    {
                        isFollowing = true;
                    }
                   
                }
                ViewBag.IsFollowing = isFollowing;
                
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
                    //imageFileName = Path.GetFileName(imageFile.FileName);
                    //var path = Path.Combine(Server.MapPath("~/Content/Images/"), imageFileName);
                    //imageFile.SaveAs(path);
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