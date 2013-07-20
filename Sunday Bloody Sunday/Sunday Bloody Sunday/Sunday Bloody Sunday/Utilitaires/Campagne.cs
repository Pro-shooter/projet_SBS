using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunday_Bloody_Sunday
{
    class Campagne
    {
        public List<String> Maps;//Id des txt
        public int progression = 0;
        public int id;

        public Campagne(List<String> Maps, int id)
        {
            this.Maps = Maps;
            this.id = id;
        }
    }
}
