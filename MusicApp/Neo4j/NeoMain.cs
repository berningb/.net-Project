﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

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
                session.Run("CREATE (a:Artist {name:" +  artist.Name + "}) SET a.Email = " + artist.Email );
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
                using(var session = driver.Session())
            {
                session.Run("CREATE (a:Song {Title:" + song.Title + "}) SET a.Filename = " + song.Filename + ", a.length" + song.length);
                session.Run("MATCH (b:Artist {name: " + artist.Name + "}), (c:Song {Title: " + song.Title + "}) CREATE (b)-[:OWN]-(c)");
            }

        }
        public void Album()
        {

        }

    }
}
