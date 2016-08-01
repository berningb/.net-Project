using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }
    }
}