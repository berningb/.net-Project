using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicApp.Interfaces;

namespace MusicApp.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Artist> Friend { get; set; }
        public List<Artist> Following { get; set; }
        public List<iLikeable> Likes { get; set; }
    }
}