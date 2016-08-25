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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Artist> Friends { get; set; }
        public List<Artist> Following { get; set; }
        public List<Artist> Followers { get; set; }
        public List<ILikeable> Likes { get; set; }

        //We need to use a constructor because we need to propery instantiate new model objects on Neo4j
        public Artist(string name, string email, string id)
        {
            Name = name;
            Email = email;
            Id = id;
            Songs = new List<Song>();
            Albums = new List<Album>();
            Playlists = new List<Playlist>();
            Friends = new List<Artist>();
            Following = new List<Artist>();
            Likes = new List<ILikeable>();

            //NeoMain.CreateArtist(this);
        }
        public Artist(string name, string email, List<Song> songs, List<Artist> Friends, List<Artist> Following, List<Artist> followers)
        {
            Name = name;
            Email = email;
            Songs = songs;
            Likes = new List<ILikeable>();
            Albums = new List<Album>();
            Playlists = new List<Playlist>();
            this.Friends = Friends;
            this.Following = Following;
            Followers = followers;
          
        }
    }
}
