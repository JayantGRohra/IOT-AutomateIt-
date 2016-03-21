using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;

using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Windows.Input;

using System.Runtime.Serialization;
using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace testproj
{
    public class Ritual
    {


        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "eventdate")]
        public DateTimeOffset EventDate { get; set; }
        [JsonProperty(PropertyName = "tubelight_value")]
        public bool Tubelight { get; set; }
        [JsonProperty(PropertyName = "coffeemachine_value")]
        public bool CoffeeMachine { get; set; }
        [JsonProperty(PropertyName = "buzzer_value")]
        public bool Buzzer { get; set; }
            




    }

}