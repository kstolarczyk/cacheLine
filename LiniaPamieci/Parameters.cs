using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiniaPamieci
{
    class Parameters
    {
        public int[] tab { get; set; }
        public int indeks { get; set; }
        public Parameters(ref int[] tab, int indeks)
        {
            this.tab = tab;
            this.indeks = indeks;
        }
    }
}
