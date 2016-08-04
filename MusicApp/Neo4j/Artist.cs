using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
   public class Artist 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Artist> Friends { get; set; }
        public List<Artist> Following { get; set; }
        public List<ILikeable> Likes { get; set; }
    }
}
