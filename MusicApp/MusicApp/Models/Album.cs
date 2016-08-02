using MusicApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Album : iLikeable, iSongCollection
    {
        public string Name { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }

        public void Add(iSongCollection collection)
        {
            //if artist == owner, add each item in collection to Songs
        }

        public void Add(Song song)
        {
            //if artist == owner, add song to Songs
        }

        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
        }
    }
}