using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class SoundSensor : Thing, ISensor
    {
        public override string Id => base.BaseId + Const.SOUND.ToLower();
        public override string Title => "Sound Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "Sound sensor detects ambient sound.";
        public override string System => Const.Safety;

        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property PressureLevel = new Property()
            {
                Title = "Sound",
                Description = "Sound Level",
                Type = "double",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.SOUND),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/sound") }
            };
            list.Add(PressureLevel);

            return list;
        }
    }
}