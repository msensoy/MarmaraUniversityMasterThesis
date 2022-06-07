using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class PIRSensor: Thing, ISensor
    {
        public override string Id => base.BaseId + Const.PIR.ToLower();
        public override string Title => "PIR Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "PIR sensor can detect infrared light emitted from objects.";
        public override string System => Const.Safety;
 
        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property TruFalseProperty = new Property()
            {
                Title = "isMovement",
                Description = "is there any movement",
                Type = "boolean",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.PIR),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/pir") }
            };
            list.Add(TruFalseProperty);
            return list;
        }

    }
}
