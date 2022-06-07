using Marmara.Common;
using Marmara.API.Concrete;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class Alarm : Thing, IActuator
    {
        public override string Id => base.BaseId + Const.ALARM.ToLower();
        public override string Title => "Alarm";
        public override string Type => Const.Actuator;
        public override string Description => "Buzzer is an acoustic warning device that works with mechanical electromechanical or piezoelectric principles.";
        public override string System => Const.Safety;
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
                Description = "Status information",
                Type = "boolean",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/status") },
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.ALARM)
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
                Description = "Alarm on/off",
                Type = "boolean",
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.actions).Href}/onoff") },

            };
            list.Add(OnOffProperty);

            return list;
        }
    }
}
