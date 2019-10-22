using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2_Estrutura
{
    class Vendas
    {
        //Codigo da venda | cliente | produto  | dataVenda no formato("yyyyMMddHHmmss")

        public int Codigo { set; get; }
        public string Cliente { get; set; }
        public int Produto { get; set; }
        public string Data { get; set; }

    }
}
