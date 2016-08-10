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
        public List<string> NeoQuery()
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                List<string> ListOutput = new List<string>();
                string output;
                // session.Run("CREATE (a:User {name:'Kurtis'}), (b:User{name:'Melissa'})");
                var result = session.Run("MATCH (n:User) RETURN n.name as name");
                foreach (var record in result)
                {
                    output = ($"{ record["name"].As<string>()}");
                    ListOutput.Add(output);
                }
                return ListOutput;
            }

        }

        public void CreateArtist(Artist artist)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                session.Run("CREATE (a:Artist {name:"+ "'"+ artist.Name + "'"+ "}) SET a.Email = " + "'"+ artist.Email +"'");
            }
        }
        public void CreatePlaylist(Playlist playlist, List<Song> songs)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
                using(var session = driver.Session())
            {
                session.Run("CREATE (a:Playlist {Title:" + playlist.Title + "} ");
            }
        }
        public void CreateSong(Song song, Artist artist)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                session.Run("CREATE (a:Song {Title:" + "'" + song.Title + "'" + " }) SET a.ImageFileName = " + "'" + song.ImageFileName + "'" );//, a.SongFileName" + "'"+ song.SongFileName +"'");
                session.Run("MATCH (b:Artist {name: " +"'"+ artist.Name +"'"+ "}), (c:Song {Title: "+"'" + song.Title + "'"+ "}) CREATE (b)-[:OWNS]->(c)");
            }

        }
            public Artist getArtist(string name)
        {

            Artist arty = null;
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
               
             var output = session.Run("MATCH (b:Artist {name: " + "'" + name + "'" + "}) return b.name as name");
                string artistName;
                foreach (var item in output)
                {
                    artistName = ($"{ item["name"].As<string>()}");
                   // string artistEmail = ($"{ item["Email"].As<string>()}");
                     arty = new Artist(artistName, artistName);
                }

            }
            return arty;

            }
        public void AddFriend(Artist fromArtist, Artist toArtist)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
                using(var session = driver.Session())
            {
                session.Run("MATCH(b:Artist {name: " + fromArtist.Name + "}), (c:Artist {name: " + toArtist.Name + "}) CREATE (b)-[:FRIEND]->(c)");
            }
        }
        public void FollowArtist(Artist follower, Artist followee)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                session.Run("MATCH(b:Artist {name: " + follower.Name + "}), (c:Artist {name: " + followee.Name + "}) CREATE (b)-[:FOLLOWS]->(c)");
            }
        }
        public List<Song> getSongs(Artist artist, string path)
        {
            List<Song> songs = new List<Song>();
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                var output = session.Run("MATCH (a:Artist {name:" + "'" + artist.Name + "'" + "})-[:OWNS]->(b:Song) RETURN b.Title as Title, b.ImageFileName as ImageFileName");

                foreach (var item in output)
                {
                    string songName = ($"{ item["Title"].As<string>()}");
                  //  string SongFileName = ($"{ item["SongFileName"].As<string>()}");
                    string ImageFileName = ($"{ item["ImageFileName"].As<string>()}");
                    
                   
                    string [] filesEntries = Directory.GetFiles(path);
                    foreach (var file in filesEntries)
                    {
                       string filename = Path.GetFileName(file);
                        if (ImageFileName == filename )
                        {
                           
                            Song song = new Song(artist, File.ReadAllBytes(file), songName, ImageFileName);
                            songs.Add(song);
                        }
                    }
                }
            }
            return songs;
        }

        public List<Artist> getFollowers(Artist artist)
        {
            List<string> outputFollowers = new List<string>();
            List<Artist> artists = new List<Artist>();

            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " + "'" + artist.Name + "'" + "})-[:FOLLOWING]-> (b:Artist) return b");
                foreach (var record in result)
                {
                    string output = ($"{ record["name"].As<string>()}");
                    outputFollowers.Add(output);
                }
                foreach (var stringOutput in outputFollowers)
                {
                    foreach (var artistsList in getAllArtist())
                    {
                        if (stringOutput == artistsList.Name)
                        {
                            Artist Finalartist = artistsList;
                            artists.Add(Finalartist);
                            
                        }
                    }
                }

            }

                return artists;
        }
        public List<Artist> getAllArtist()
        {
            List<Artist> artistList = new List<Artist>();
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
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
        public List<Artist> getFriends(Artist artist)
        {
            List<string> outputFollowers = new List<string>();
            List<Artist> artists = new List<Artist>();

            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                var result = session.Run("MATCH (a:Artist {name: " +"'"+artist.Name +"'" +"})-[:FRIENDS]-> (b:Artist) return b");
                foreach (var record in result)
                {
                    string output = ($"{ record["name"].As<string>()}");
                    outputFollowers.Add(output);
                }
                foreach (var stringOutput in outputFollowers)
                {
                   foreach(var artistsList in getAllArtist())
                    {
                        if(stringOutput == artistsList.Name)
                        {
                            Artist Finalartist = artistsList;
                            artists.Add(Finalartist);
                        }
                    }
                }
            }
            return artists;
        }
        public void Album()
        {

        }

    }
}
