using MusicApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Song : iLikeable
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