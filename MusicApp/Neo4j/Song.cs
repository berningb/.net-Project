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
        public HttpPostedFile File { get; set; }
        public HttpPostedFile Image { get; set; }
        public string SongFileName { get; set; }
        public string ImageFileName { get; set; }
        public string Title { get; set; }
        public double length { get; set; }

      

        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
            Likees.Add(artist);
        }

    }
}
