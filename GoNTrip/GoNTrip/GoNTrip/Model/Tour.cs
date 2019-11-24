﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject]
    public class Tour : ModelElement
    {
        [CheckTourGuidingAbilityField("tourId")]
        [JoinPrepareField("tourId")]
        [CheckTourJoinAbilityField("tourId")]
        [JoinTourField("tourId")]
        [GetTourMembersField]
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
        public List<Participating> participatingList { get; set; }
        public Guide guide { get; set; }

        public Tour() { }
    }
}
