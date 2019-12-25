using System;

using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    public class CustomTour : ModelElement
    {
        [CustomTourCreate("name")]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [CustomTourCreate("description")]
        [JsonProperty("description")]
        public string Description { get; private set; }

        [CustomTourCreate("location")]
        [JsonProperty("location")]
        public string Location { get; private set; }

        [CustomTourCreate("startDateTime")]
        [JsonProperty("startDateTime")]
        public DateTime StartDateTime { get; private set; }

        [CustomTourCreate("finishDateTime")]
        [JsonProperty("finishDateTime")]
        public DateTime FinishDateTime { get; private set; }

        public CustomTour(string name, string description, string location, DateTime startDateTime, DateTime finishDateTime)
        {
            Name = name;
            Description = description;
            Location = location;
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
