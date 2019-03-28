using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiniaPamieci
{
    class Parameters
    {
        unsafe public int* tab { get; set; }
        public int indeks { get; set; }
        unsafe public Parameters(int * tab, int indeks)
        {
            this.tab = tab;
            this.indeks = indeks;
        }
    }
}
