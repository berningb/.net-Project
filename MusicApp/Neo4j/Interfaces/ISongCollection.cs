using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.Interfaces
{
   public interface ISongCollection
    {
        string Title { get; set; }
        List<Song> Songs { get; set; }
        void Add(Song song);
        void Add(ISongCollection collection);
    }
}
