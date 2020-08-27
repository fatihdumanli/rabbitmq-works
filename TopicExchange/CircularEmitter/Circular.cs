using Randomizer;

namespace CircularEmitter
{
    public class Circular : IRandomizable
    {
        public string Location { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }

        public string GetRouteKey()
        {
            return string.Format("{0}.{1}", Location.ToLower(), Department.ToLower());
        }
        
    }
}