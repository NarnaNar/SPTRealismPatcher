using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTRealismPatcher
{
    internal class ItemToAdd
    {
        public ItemToAdd(string itemID, string itemName, string itemTplToClone)
        {
            ItemID = itemID;
            ItemName = itemName;
            ItemTplToClone = itemTplToClone;
        }

        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemTplToClone { get; set; }

        [JsonExtensionData]
        public JObject Properties { get; set; }
    }
}
