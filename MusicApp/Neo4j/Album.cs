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

        //We need to use a constructor because we need to propery instantiate new model objects on Neo4j
        public Album(string title, Artist owner)
        {
            Title = title;
            Owner = owner;
            Songs = new List<Song>();
            Likees = new List<Artist>();

            // Neo4j new Album(this)
            // Neo4j owner -Albums-> this(Album)
        }

        public void Add(ISongCollection collection)
        {
            /*
            if (Owner == currentUser)
            {
                Songs.AddRange(collection.Songs);
                foreach (Song song in collection.Songs)
                {
                    // Neo4j this(Album) -Songs-> song
                }
            }
            */
        }
        public void Add(Song song)
        {
            /*
            if (Owner == currentUser)
            {
                Songs.Add(song)
                // Neo4j this(Album) -Songs-> song
            }
            */
        }
        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
            // Neo4j artist -likes-> this(Album)

            Likees.Add(artist);
            // Neo4j this(Album) -likees-> artist
        }


    }
}
