using MusicApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Playlist : iLikeable, iSongCollection
    {
        public string Name { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }

        public void Add(iSongCollection collection)
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