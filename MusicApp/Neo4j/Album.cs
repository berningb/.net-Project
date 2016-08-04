using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
   public class Album : ILikeable, ISongCollection
    {
        public string Title { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }
        public List<Artist> Likees { get; set; }

        public void Add(ISongCollection collection)
        {
            //if artist == owner
            //Songs.AddRange(collection.Songs);
        }
        public void Add(Song song)
        {
            //if artist == owner
            //Songs.Add(song);
        }
        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
            Likees.Add(artist);
        }


    }
}
