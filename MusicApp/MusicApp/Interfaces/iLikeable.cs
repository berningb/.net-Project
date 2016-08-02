using MusicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Interfaces
{
    public interface iLikeable
    {
        void Like(Artist artist);
    }
}