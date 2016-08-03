﻿using Neo4j.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j
{
   public class Album : ILikeable, ISongCollection
    {
        public string Name { get; set; }
        public Artist Owner { get; set; }
        public List<Song> Songs { get; set; }

        public void Add(ISongCollection collection)
        {
            //if artist == owner, add each item in collection to Songs
        }
        public void Add(Song song)
        {
            //if artist == owner, add song to songs
        }
        public void Like(Artist artist)
        {
            artist.Likes.Add(this);
        }


    }
}