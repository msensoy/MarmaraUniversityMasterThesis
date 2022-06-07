using Marmara.Common;
using Marmara.API.Concrete;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class CFL : Thing, IActuator
    {
        public override string Id => base.BaseId + Const.CFL.ToLower();
        public override string Title => "Compact Fluorescent Lamp";
        public override string Type => Const.Actuator;
        public override string Description => "The compact fluorescent light bulb or lamp is a type of fluorescent lamp generally designed as a replacement for incandescent or halogen lamps.";
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
            Property OnOffProperty = new Property()
            {
                Title = "statu",
                Description = "Status information whether the lamp is on",
                Type = "boolean",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/status") },
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.CFL)

            };
            list.Add(OnOffProperty);

            return list;
        }
        private IEnumerable<Action> AddAction()
        {
            var list = new List<Action>();
            Action OnOffProperty = new Action()
            {
                Title = "onoff",
                Description = "Lamp on/off",
                Type = "boolean",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.actions).Href}/onoff") },

            };
            list.Add(OnOffProperty);

            return list;
        }

    }
}

