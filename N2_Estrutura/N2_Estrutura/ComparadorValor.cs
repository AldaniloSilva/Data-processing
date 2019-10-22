using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace N2_Estrutura
{
    class ComparadorValor : IComparer<CategoriaAuxiliar>
    {
        public int Compare(CategoriaAuxiliar x, CategoriaAuxiliar y)
        {
            return x.Indice.CompareTo(y.Indice);
        }
    }
}
