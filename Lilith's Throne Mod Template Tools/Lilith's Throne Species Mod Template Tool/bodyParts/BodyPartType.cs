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
        public NestedNode tags;
        public BodyPartType(string name = null, List<string> availableCoveringTypes=null, string race=null) : base (name, new Dictionary<string, ManagedXmlNode>(), true)
        {
            this.name = name;
            this.AvailableCoveringTypes = availableCoveringTypes;
            this.race = race;
        }

        public void AddTag(string tagName)
        {
            this.tags.AddStringSubnode("tag", tagName, true, false);
        }
    }
}
