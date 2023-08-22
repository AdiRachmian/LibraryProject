using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    [Flags]
    public enum Genres
    {
        Fiction = 1,
        Children = 2,
        Poetry = 4,
        YoungAdult = 8,
        History = 16,
        Newspaper = 32,
        Cooking = 64,
        Art = 128,
        Fashion = 256,
        Thriller = 512
    }
}
