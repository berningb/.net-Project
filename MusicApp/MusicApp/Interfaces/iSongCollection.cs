using MusicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Interfaces
{
    public interface iSongCollection
    {
        void Add(Song song);
        void Add(iSongCollection collection);
    }
}