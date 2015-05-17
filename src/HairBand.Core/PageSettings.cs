using System.Dynamic;

namespace HairBand
{
    public class PageSettings : DynamicObject
    {
        public PageSettings()
        {
        }

        public string Title { get; set; }

    }
}