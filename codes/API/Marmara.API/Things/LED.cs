using Marmara.Common;
using Marmara.API.Concrete;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class LED : Thing, IActuator
    {
        public override string Id => base.BaseId + Const.LED.ToLower();
        public override string Title => "Dimmable Led";
        public override string Type => Const.Actuator;
        public override string Description => "Dimming LED lamps can save energy and changes the visual appearance and mood of your space.";
        public override string System => Const.Lighting;
        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }
        public override IEnumerable<Action> Actions
        {
            get => AddAction();
            set => base.Actions = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();

            Property BrightnessProperty = new Property()
            {
                Title = "Brightness",
                Description = "0-100 light level",
                Type = "integer",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.LED),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/brightness") }
            };
            list.Add(BrightnessProperty);

            return list;
        }
        private IEnumerable<Action> AddAction()
        {
            var list = new List<Action>();
            Action OnOffProperty = new Action()
            {
                Title = "set",
                Description = "Set brightness",
                Type = "integer",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.actions).Href}/set") },

            };
            list.Add(OnOffProperty);


            return list;
        }
    }
}
