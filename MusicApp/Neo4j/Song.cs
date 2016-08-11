using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Neo4j
{
  public class Song: ILikeable
    {
        public Artist Owner { get; set; }
        public List<Artist> Likees { get; set; }
        public byte[] file { get; set; }
        public byte[] image { get; set; }
        public string SongFileName { get; set; }
        public string ImageFileName { get; set; }
        public string filename { get; set; }
        public string Title { get; set; }
        public double Length { get; set; }
        public string imageURL { get; set; }

        //We need to use a constructor because we need to propery instantiate new model objects on Neo4j
        public Song(Artist owner, HttpPostedFileBase file, HttpPostedFileBase image, string songFileName, string imageFIleName, string title, double length)
        {
            Owner = owner;
            Likees = new List<Artist>();
            
            SongFileName = songFileName;
            ImageFileName = imageFIleName;
            Title = title;
            Length = length;

           // NeoMain.CreateSong(this);
        }

        public Song(Artist owner, byte[] image, string title, string filename)
        {
            
            this.Owner = owner;
            Title = title;
            this.filename = filename;
            this.ImageFileName = filename;
           // this.SongFileName = filename;
            this.image = image;
            
        }
        public Song(Artist owner, string title, string filename)
        {
            Owner = owner;
            Title = title;
            ImageFileName = filename;
        }
        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
            Likees.Add(artist);
            //NeoMain.Like(artist, this);
        }
    }
}
