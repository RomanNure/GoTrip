﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model.FilterSortSearch.Tour
{
    [JsonObject]
    public class TourSemiFilters
    {
        [JsonIgnore]
        private long DEFAULT_ID = -1;

        [JsonIgnore]
        private bool DEFAULT_BOOL = false;

        public long tourGuideId { get; set; }
        public long tourMemberId { get; set; }
        public bool withApprovedGuideOnly { get; set; }
        public bool noApprovedGuideOnly { get; set; }

        public TourSemiFilters() => ResetAll();

        private void ResetAll()
        {
            tourGuideId = DEFAULT_ID;
            tourMemberId = DEFAULT_ID;
            Reset();
        }

        public void Reset()
        {
            withApprovedGuideOnly = DEFAULT_BOOL;
            noApprovedGuideOnly = DEFAULT_BOOL;
        }

        [JsonIgnore]
        public bool IsChanged => tourGuideId != DEFAULT_ID || tourMemberId != DEFAULT_ID ||
                                 withApprovedGuideOnly != DEFAULT_BOOL || noApprovedGuideOnly != DEFAULT_BOOL;
    }
}