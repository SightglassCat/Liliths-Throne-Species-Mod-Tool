using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedXML;

namespace LTSMTT.bodyParts
{
    partial class BodyPartType : NestedNode
    {
        public List<string> AvailableCoveringTypes { get; set; }
        public string race { get; set; }
        public BodyPartType(string name = null, List<string> availableCoveringTypes=null, string race=null)
        {
            this.name = name;
            this.AvailableCoveringTypes = availableCoveringTypes;
            this.race = race;
        }
    }
}
