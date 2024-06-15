using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTRealismPatcher
{
    internal class RealismTemplate
    {
        public RealismTemplate(string itemID, string name, JObject properties, string localeName, string origFileName)
        {
            ItemID = itemID;
            Name = name;
            Properties = properties;
            LocaleName = localeName;
            OrigFileName = origFileName;
        }

        [JsonIgnore]
        public string LocaleName { get; set; }

        [JsonIgnore]
        public string OrigFileName { get; set; }

        public string ItemID { get; set; }
        public string Name { get; set; }

        [JsonExtensionData]
        public JObject Properties { get; set; }
    }

}
