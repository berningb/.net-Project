using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Neo4j
{
  public class Song: ILikeable
    {
        public Artist Owner { get; set; }
        public List<Artist> Likees { get; set; }
        public HttpPostedFileBase File { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string SongFileName { get; set; }
        public string ImageFileName { get; set; }
        public string Title { get; set; }
        public double Length { get; set; }

        //We need to use a constructor because we need to propery instantiate new model objects on Neo4j
        public Song(Artist owner, HttpPostedFileBase file, HttpPostedFileBase image, string title, double length)
        {
            Owner = owner;
            //Likees = new List<Artist>();
            File = file;
            Image = image;
            Title = title;
            Length = length;

           // NeoMain.CreateSong(this);
        }

        public Song(Artist owner, HttpPostedFileBase file, HttpPostedFileBase image, string title)
        {
            Owner = owner;
            File = file;
            Image = image;
            Title = title;
            SongFileName = file.FileName;
            ImageFileName = image.FileName;

        }
        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
            Likees.Add(artist);
            //NeoMain.Like(artist, this);
        }
    }
}
