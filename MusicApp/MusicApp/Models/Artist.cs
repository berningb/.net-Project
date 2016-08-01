using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Song> Album { get; set; }
        public List<Song> Playlist { get; set; }
        public List<Artist> Friend { get; set; }
        public List<Artist> Following { get; set; }
    }
}