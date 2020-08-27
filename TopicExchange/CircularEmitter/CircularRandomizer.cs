using Randomizer;

namespace CircularEmitter
{
    public class CircularRandomizer : IRandomizer
    {
        string[] locations = { "istanbul", "izmir", "ankara", "samsun", "adana", "kocaeli", "mersin" };
        string[] departments = { "operasyon", "bt-analiz", "gise", "nakit-yonetimi", "crm", "guvenlik" };

        public IRandomizable CreateRandomizeInstance()
        {
            var circular = new Circular();
            circular.Location = new RandomPicker<string>().Pick(locations);
            circular.Department = new RandomPicker<string>().Pick(departments);
            circular.Description = string.Format(" [ {0}.{1} ]This is sample description for the circular.", circular.Location, circular.Department);
            return circular;           
        }

    }
}