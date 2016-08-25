using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
   public class Playlist 
    {
        public string Title { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }
       

        //We need to use a constructor because we need to propery instantiate new model objects on Neo4j
        public Playlist(string title, Artist owner)
        {
            Title = title;
            Owner = owner;
            Songs = new List<Song>();
         

          //  NeoMain.CreatePlaylist(this);
        }
        public void Add(ISongCollection collection, Artist currentUser)
        {
            foreach (Song song in collection.Songs)
            {
                Add(song, null);
            }
        }
        public void Add(Song song, Artist currentUser)
        {
            Songs.Add(song);
            //NeoMain.AddSongToCollection(this, song, null);
        }
      
    }
}

