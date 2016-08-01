using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Interfaces
{
    interface SongInterface
    {
        void addSong(Song song);
        void deleteSong(int id);
        void editSong(Song s);
    }
}
