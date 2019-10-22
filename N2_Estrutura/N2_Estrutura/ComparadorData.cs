using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2_Estrutura
{
    class ComparadorData : IComparer <VendasPorData>
    {
        public int Compare(VendasPorData x, VendasPorData y)
        {
            return x.Data.CompareTo(y.Data);
        }
    }
}
