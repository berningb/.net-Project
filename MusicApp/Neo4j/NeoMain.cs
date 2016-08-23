using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using System.IO;
using web = System.Web;
using System.Web.UI.WebControls;

namespace Neo4j
{
    public class NeoMain
    {
        private string[] boltEndpoint = new string[] { "bolt://localhost", "bolt://sb10.stations.graphenedb.com:24786", "bolt://neo-trial-lillie-sauer-orangered.azure.graphstory.com:7687" };
        private string[,] authTokens = new string[,] { { "neo4j", "test" }, { "neo4jdb", "Wdd9t4d4ccK72FeXIttm" }, { "neo_trial_lillie_sauer_orangered", "fLYPzrTSQHda4CXtGcxG1ntvo4yh0jc0TO76mhnf" } };


        public void CreateArtist(Artist artist)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                session.Run("CREATE (a:Artist {name:"+ "'"+ artist.Name + "'"+ "}) SET a.Email = " + "'"+ artist.Email +"'");
            }
        }
        public void AddProfilePicture(Artist artist)
           
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                session.Run("MATCH (a:Artist {name:" + "'" + artist.Name + "' " + "}) SET a.ProfilePicture = " + "'" + artist.ProfilePicture + "'");

            }

        }
        public string GetProfilePicture(Artist artist)
        {
            string ProfilePicture = null;
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
               
            {
                
                var output = session.Run("MATCH (a:Artist {name:" + "'" + artist.Name + "' " + "}) RETURN a.ProfilePicture as ProfilePicture");
              
                foreach (var item in output)
                {
                    ProfilePicture = ($"{ item["ProfilePicture"].As<string>()}");
                }

            }
            return ProfilePicture;
        }
        public void CreatePlaylist(Playlist playlist, List<Song> songs)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
                using(var session = driver.Session())
            {
                session.Run("CREATE (a:Playlist {Title:" + playlist.Title + "} ");
            }
        }
        public void CreateSong(Song song, Artist artist)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                session.Run("CREATE (a:Song {Title:" + "'" + song.Title + "'" + " }) SET a.ImageFileName = " + "'" + song.ImageFileName + "'" + ", a.SongFileName = " + "'" + song.SongFileName + "'" );
                session.Run("MATCH (b:Artist {name: " +"'"+ artist.Name +"'"+ "}), (c:Song {Title: "+"'" + song.Title + "'"+ "}) CREATE (b)-[:OWNS]->(c)");
            }

        }
        public void LikeSong(Artist artist, Song song)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                session.Run("MATCH (a:Artist {name: " + "'" + artist.Name + "'" + "}), (b:Song {Title:" + "'" + song.Title + "'" + "}) CREATE (a)-[:LIKES]->(b)");
            }
        }
        public Song getLikes(string songTitle)
        {
            Song newSong = null;
            List<Artist> ArtistWhoLikedThisSong = new List<Artist>();
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var output = session.Run("MATCH (b:Song {Title: " + "'" + songTitle + "'" + "})-[:LIKES]->(c:Artist) RETURN c.name as name, c.Email as Email");
                int count = 0;
                Artist artist = null;
                foreach(var item in output)
                {
                    count++;
                  string ArtistName = ($"{ item["name"].As<string>()}");
                  string Email = ($"{ item["Email"].As<string>()}");

                     artist = new Artist(ArtistName, Email);
                    ArtistWhoLikedThisSong.Add(artist);
                }
            }
            return newSong;
        }
        public Artist getArtist(string name)
        {

            Artist arty = null;
            Artist finalArtist = null;
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
               
             var output = session.Run("MATCH (b:Artist {name: " + "'" + name + "'" + "}) return b.name as name, b.Email as Email");
                string artistName;
                foreach (var item in output)
                {
                    artistName = ($"{ item["name"].As<string>()}");
                   string artistEmail = ($"{ item["Email"].As<string>()}");
                     arty = new Artist(artistName, artistEmail);
                     
                }
                finalArtist = new Artist(arty.Name, arty.Email, getSongs(arty), getFriends(arty), getPeopleYouFollow(arty), getFollowers(arty));
            }
            return finalArtist;
            }
        public void AddFriend(Artist fromArtist, Artist toArtist)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
                using(var session = driver.Session())
            {
                session.Run("MATCH(b:Artist {name: " +"'" + fromArtist.Name + "'" + "}), (c:Artist {name: " +"'" + toArtist.Name + "'"+ "}) CREATE (b)-[:FRIEND]->(c)");
            }
        }
        public void RemoveFiend(Artist fromArtist, Artist toArtist)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                session.Run("MATCH(b:Artist{name: " + "'" + fromArtist.Name + "'" + "}), (c:Artist {name: " + "'" + toArtist.Name + "'" + "}), (b)-[r:FRIEND]->(c) DELETE r");

            }
        }
        public void Unfollow(Artist fromArtist, Artist toArtist)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                session.Run("MATCH(b:Artist{name: " + "'" + fromArtist.Name + "'" + "}), (c:Artist {name: " + "'" + toArtist.Name + "'" + "}), (b)-[r:FOLLOWING]->(c) DELETE r");

            }
        }
        public void FollowArtist(Artist follower, Artist followee)
        {
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                session.Run("MATCH(b:Artist {name: " +"'" + follower.Name +"'"+ "}), (c:Artist {name: " +"'"+ followee.Name +"'" + "}) CREATE (b)-[:FOLLOWING]->(c)");
            }
        }


        
        public List<Song> getSongs(Artist artist)
        {
            List<Song> songs = new List<Song>();
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var output = session.Run("MATCH (a:Artist {name:" + "'" + artist.Name + "'" + "})-[:OWNS]->(b:Song) RETURN b.Title as Title, b.ImageFileName as ImageFileName, b.SongFileName as SongFileName");

                foreach (var item in output)
                {
                    string songName = ($"{ item["Title"].As<string>()}");
                    //string ImageFileName = ($"{ item["ImageFileName"].As<string>()}");
                    //string SongFileName = ($"{ item["SongFileName"].As<string>()}");

                    Song song = new Song(artist, songName);//, ImageFileName, SongFileName);
                    songs.Add(song);              
                }
            }
            return songs;
        }
        public List<Artist> getPeopleYouFollow(Artist artist)
        {
            List<Artist> artists = new List<Artist>();

            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " + "'" + artist.Name + "'" + "})-[r:FOLLOWING]->(b:Artist) return b.name as name, b.Email as Email");
                foreach (var record in result)
                {
                    string output = ($"{ record["name"].As<string>()}");
                    string Email = ($"{ record["Email"].As<string>()}");
                    Artist artist1 = new Artist(output, Email);
                    artists.Add(artist1);
                }

            }
            return artists;

       }
        public List<Song> getMostLiked()
        {
            List<Song> songs = new List<Song>();
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist)-[:LIKES]->(s:Song) WITH a,count(s) as rels, collect(s) as songs WHERE rels > 1 RETURN a, songs.Name as Name, songs.Owner as Owner, rels");

            }
            return songs;
        }

        public List<Artist> getFollowers(Artist artist)
        {
          
            List<Artist> artists = new List<Artist>();

            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " + "'" + artist.Name + "'" + "})<-[:FOLLOWING]-(b:Artist) return b.name as name, b.Email as Email");
                foreach (var record in result)
                {
                    string output = ($"{ record["name"].As<string>()}");
                    string Email = ($"{ record["Email"].As<string>()}");
                    Artist artist1 = new Artist(output, output);
                    artists.Add(artist1);
                }
                 
            }
            return artists;
        }
        public bool isFollowing(Artist mainArtist, Artist searchedArtist)
        {
            bool isfollow = false;
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " + "'" + mainArtist.Name + "'" + "})-[:FOLLOWING]->(b:Artist {name:" + "'" + searchedArtist.Name + "'" + " }) RETURN b.Name as Name");

                foreach(var record in result)
                {
                    string name = ($"{ record["name"].As<string>()}");
                    if(name == searchedArtist.Name)
                    {
                        isfollow = true;
                    }
                    else
                    {
                        isfollow = false;
                    }
                }

            }
            return isfollow;
        }
        public List<Artist> getAllArtist()
        {
            List<Artist> artistList = new List<Artist>();
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (n:Artist) return n");

                foreach (var item in result)
                {
                    string name = ($"{ item["name"].As<string>()}");
                    string email = ($"{ item["Email"].As<string>()}");

                    Artist artist = new Artist(name, email);
                    artistList.Add(artist);
                }

            }
            return artistList;
        }
        public List<Song> getAllSongs()
        {
            List<Song> songList = new List<Song>();
            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2, 0], authTokens[2, 1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH(b:Artist)-[:OWNS]->(n:Song) return b.name as name, n.Title as Title");



                foreach (var item in result)
                {
                    string Title = ($"{ item["Title"].As<string>()}");
                    string artist = ($"{ item["name"].As<string>()}");

                    //string owner = ($"{ item["owner"].As<string>()}");
                    // Artist arty = getArtist(owner);
                    Song song = new Song(getArtist(artist), Title);
                    songList.Add(song);

                }
            }
            return songList;

        }

        public List<Artist> getFriends(Artist artist)
        {
           
            List<Artist> artists = new List<Artist>();

            using (var driver = GraphDatabase.Driver(boltEndpoint[2], AuthTokens.Basic(authTokens[2,0], authTokens[2,1])))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " + "'" + artist.Name + "'" + "})-[:FRIEND]-> (b:Artist) return b.name as name, b.Email as Email");
                foreach (var record in result)
                {
                    string output = ($"{ record["name"].As<string>()}");
                    string Email = ($"{ record["Email"].As<string>()}");
                    Artist artist1 = new Artist(output, output);
                    artists.Add(artist1);
                }
            }
            return artists;
        }
        public void Album()
        {

        }

    }
}
