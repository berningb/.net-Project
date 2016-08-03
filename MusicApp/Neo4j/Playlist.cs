using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
   public class Playlist : ILikeable, ISongCollection
    {
        public string Name { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }

        public void Add(ISongCollection collection)
        {
            //itterate through colcetion, add each
        }

        public void Add(Song song)
        {
            Songs.Add(song);
        }

        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
        }
    }
}

