using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.Interfaces
{
   public interface ILikeable
    {
        string Title { get; set; }
        List<Artist> Likees { get; set; }
        void Like(Artist artist);
    }
}
