using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model
{
    public class Participating : ModelElement
    {
        public int id { get; set; }
        public int tourRate { get; set; }
        public int guideRate { get; set; }
        public string hash { get; set; }

        public Participating() { }
    }
}
