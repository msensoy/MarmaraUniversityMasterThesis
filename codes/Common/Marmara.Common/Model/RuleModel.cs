namespace Marmara.Common.Model
{
    public class RuleModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsGreaterThen { get; set; }
        public int Value { get; set; }
        public int ValueActive { get; set; }
        public int ValueGreaterThen { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} - IsActive: {IsActive.ToString()} - IsGreaterThen: {IsGreaterThen} - Value:{Value} - ValueActive:{ValueActive} - ValueGreaterThen:{ValueGreaterThen}";
        }

    }
}
