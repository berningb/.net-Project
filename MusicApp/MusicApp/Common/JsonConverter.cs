using Neo4j;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Common
{
    public class JsonConverter
    {
        public static string Serialize(Artist arty)
        {
            string output = JsonConvert.SerializeObject(arty);
            return output;
        }

        //public static Artist DeSerialize(string arty)
        //{
        //    Artist artyDeserialized = JsonConvert.DeserializeObject<Artist>(arty);
        //    return artyDeserialized;
        //}
    }
}