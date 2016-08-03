using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.Interfaces
{
   public interface ISongCollection
    {
        void Add(Song song);
        void Add(ISongCollection collection);
    }
}
