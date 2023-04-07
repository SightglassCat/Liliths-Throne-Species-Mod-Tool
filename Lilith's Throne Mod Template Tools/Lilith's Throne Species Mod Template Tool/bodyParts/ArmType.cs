using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedXML;

namespace LTSMTT.bodyParts
{
    class ArmType : BodyPartType
    {
        public ArmType(string name="arm", List<string> availableCoveringTypes = null, string race = null) : base(name, availableCoveringTypes, race)
        {
            this.AddStringSubnode("race", race);
            this.AddStringSubnode("coveringType", "HUMAN");
            this.AddCDataSubnode("transformationName", "new arm");
            this.AddBooleanSubnode("underarmHairAllowed", true, false);
            this.AddBooleanSubnode("allowsFlight", false);
        }
    }
}
