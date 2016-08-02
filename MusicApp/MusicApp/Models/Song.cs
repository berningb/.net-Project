using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Song
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public Artist Owner { get; set; }
        public double length { get; set; }

    }
}