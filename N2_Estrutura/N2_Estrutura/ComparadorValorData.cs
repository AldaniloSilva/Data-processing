using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2_Estrutura
{
    class ComparadorValorData : IComparer<VendasPorData>
    {
        public int Compare(VendasPorData x, VendasPorData y)
        {
            return x.Total.CompareTo(y.Total);
        }
    }
}
