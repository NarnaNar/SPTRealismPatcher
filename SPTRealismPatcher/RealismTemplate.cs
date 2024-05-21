﻿using Newtonsoft.Json;
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
        public RealismTemplate(string itemID, string name, JObject properties)
        {
            ItemID = itemID;
            Name = name;
            Properties = properties;
        }

        public string ItemID { get; set; }
        public string Name { get; set; }

        [JsonExtensionData]
        public JObject Properties { get; set; }
    }

}