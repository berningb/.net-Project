using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
  public class Song: ILikeable
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public Artist Owner { get; set; }
        public double length { get; set; }

        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
        }

    }
}
