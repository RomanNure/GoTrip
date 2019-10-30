using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Tour : ModelElement
    {
        public long id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public double pricePerPerson { get; set; }
        public string mainPictureUrl { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime finishDateTime { get; set; }
        public int maxParticipants { get; set; }
        public Admin administrator { get; set; }
        public List<Photo> photos { get; set; }
        public List<object> participatingList { get; set; }

        public Tour() { }
    }
}
