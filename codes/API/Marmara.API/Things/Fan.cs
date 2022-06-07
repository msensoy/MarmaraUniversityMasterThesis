using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class Fan : Thing, IActuator
    {
        public override string Id => base.BaseId + Const.FAN.ToLower();
        public override string Title => "Fan";
        public override string Type => Const.Actuator;
        public override string Description => "The fan is a powerful machine used to create airflow.";
        public override string System => Const.HVAC;

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
            Property TruFalseProperty = new Property()
            {
                Title = "isWorking",
                Description = "is Working",
                Type = "boolean",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.FAN),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/fan") }
            };
            list.Add(TruFalseProperty);
            return list;
        }
        private IEnumerable<Action> AddAction()
        {
            var list = new List<Action>();
            Action OnOffProperty = new Concrete.Action()
            {
                Title = "onoff",
                Description = "Alarm on/off",
                Type = "boolean",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.actions).Href}/onoff") },

            };
            list.Add(OnOffProperty);

            return list;
        }
    }
}