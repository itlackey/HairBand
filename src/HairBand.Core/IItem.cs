using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface IItem<TKey>
    {
        TKey Id { get; set; }
        string Name { get; set; }
    }
}
