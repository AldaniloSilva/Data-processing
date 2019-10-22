using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace N2_Estrutura
{
    class Program
    {
        static CategoriaAuxiliar AuxiliarCat(string categoria, double total, int codigo)
        {
            CategoriaAuxiliar categoriaAuxiliar = new CategoriaAuxiliar();
            categoriaAuxiliar.Indice = int.Parse(categoria.Substring(categoria.IndexOf("- ") + 1));
            categoriaAuxiliar.Descricao = categoria;
            categoriaAuxiliar.Codigo = codigo;
            categoriaAuxiliar.Total = total;
            return categoriaAuxiliar;
        }
        static DateTime FormataData(string dat)
        {
            string dataAux = dat.Insert(4, "/");           
            var dt = DateTime.Parse(dataAux).ToString("MM-yyyy");
            DateTime data = Convert.ToDateTime(dt);
            return data;
        }

        static Vendas ArmazenaVendas(int cod, string cliente, int prod, string dat)
        {            
            Vendas arm = new Vendas();
            arm.Codigo = cod;
            arm.Cliente = cliente;
            arm.Produto = prod;
            arm.Data = dat;

            return arm;
        }
        static string Substring(string x)
        {
            string valor = x.Substring(0, x.IndexOf('|'));
            return valor;
        }
        static VendasPorCodigo ArmazenaVendasCod(int cod)
        {
            VendasPorCodigo v = new VendasPorCodigo();
            v.Id = cod;        
            return v;
        }

        static VendasPorData ArmazenaVendasData(DateTime dt, double total)
        {
            var dta = new VendasPorData();
            dta.Data = dt;
            dta.Total = total;
            return dta;
        }
        static void Main(string[] args)
        {
            DateTime inicio = DateTime.Now;
            File.AppendAllText("resultado.txt", DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine);// guarda  a hora inicial
            Dictionary<int, string> Categorias = new Dictionary<int, string>();
            Dictionary<int, Produtos> Produtos = new Dictionary<int, Produtos>();
            Dictionary<string, string> Clientes = new Dictionary<string, string>();            
            StringBuilder r = new StringBuilder();
            Dictionary<string, double> ComprasClientes = new Dictionary<string, double>();
            Dictionary<DateTime, VendasPorData> VendasData = new Dictionary<DateTime, VendasPorData>();
            List<Vendas> vendas2 = new List<Vendas>();
            HashSet<int> Vendinhaaa = new HashSet<int>();
            string linha;

            //Lê o arquivo e mostra linha por linha
            #region A.Quantidade total de categorias
            StreamReader leitura =
                new StreamReader("categorias.txt");
            while ((linha = leitura.ReadLine()) != null)
            {
                //Lê a linha e pega apenas o código da categoria até o '|'
                int x = int.Parse(linha.Substring(0, linha.IndexOf('|')));
                //Adiciona a Categoria no dicionário
                if (!Categorias.ContainsKey(x))
                    Categorias.Add(x, linha.Substring(linha.IndexOf('|')));
            }

            leitura.Close();
            File.AppendAllText("resultado.txt", "A- " + Categorias.Count.ToString() + Environment.NewLine);
            Console.WriteLine("A: {0}", Categorias.Count);
            #endregion

            #region B.Quantidade total de produtos cadastrados    
            leitura = new StreamReader("produtos.txt");
            while ((linha = leitura.ReadLine()) != null)
            {
                Produtos prod = new Produtos();
                prod.Categoria = int.Parse(linha.Split('|')[3]);
                prod.Codigo = int.Parse(linha.Split('|')[0]);
                if (!Categorias.ContainsKey(prod.Categoria) || Produtos.ContainsKey(prod.Codigo))
                    continue;
                prod.Nome = linha.Split('|')[2];
                prod.Preco = double.Parse(linha.Split('|')[1]);
                Produtos.Add(prod.Codigo, prod);
            }
            leitura.Close();
            File.AppendAllText("resultado.txt", "B- " + Produtos.Count.ToString() + Environment.NewLine);
            Console.WriteLine("B: {0}", Produtos.Count);

            #endregion

            #region C.Quantidade total de clientes  

            leitura = new StreamReader("clientes.txt");
            while ((linha = leitura.ReadLine()) != null)
            {
                string[] dadosclientes = linha.Split('|');
                if (!Clientes.ContainsKey(dadosclientes[0]))
                    Clientes.Add(dadosclientes[0], dadosclientes[1]);
            }
            leitura.Close();

            File.AppendAllText("resultado.txt", "C- " + Clientes.Count.ToString() + Environment.NewLine);
            Console.WriteLine("C: {0}", Clientes.Count);
            #endregion

            #region D. Quantidade distinta de vendas individuais (sem repetir o código da venda)
            
            leitura = new StreamReader("vendas2.txt");
            linha = leitura.ReadLine();
            while (linha != null)
            {
                int codigo = int.Parse(Substring(linha));
                linha = linha.Substring(linha.IndexOf('|') + 1);
                string cliente = Substring(linha);
                linha = linha.Substring(linha.IndexOf('|') + 1);
                int produto = int.Parse(Substring(linha));
                linha = linha.Substring(linha.IndexOf('|') + 1);
                string data = linha.Substring(0,6);

                if (Produtos.ContainsKey(produto)) // se contém a chave (código do produto)
                    if (Clientes.ContainsKey(cliente))
                    {
                        vendas2.Add(ArmazenaVendas(codigo, cliente, produto, data));
                        Vendinhaaa.Add(codigo);
                    }

                linha = leitura.ReadLine();
            }
            leitura.Close();
            File.AppendAllText("resultado.txt", "D- " + Vendinhaaa.Count + Environment.NewLine);
            Console.WriteLine("D: {0}", Vendinhaaa.Count);
            #endregion

            #region E.Quantidade distinta de produtos vendidos (sem repetir o código do produto)   
            DateTime dtaAux;
            List<VendasPorCodigo> Vendinha1 = new List<VendasPorCodigo>();
            Dictionary<int, int> ProdutosVendidos = new Dictionary<int, int>();

            int vendcodigo = vendas2[0].Codigo;

            VendasPorCodigo vendinfo = ArmazenaVendasCod(vendcodigo);
            Vendinha1.Add(vendinfo);
            foreach (var vendas in vendas2)
            {
                if (!ProdutosVendidos.ContainsKey(vendas.Produto))
                {
                    ProdutosVendidos.Add(vendas.Produto, 0);
                }

                #region Colhendo informações para o OpCode G
                if (ComprasClientes.ContainsKey(vendas.Cliente)) // se já existir o cpf do cliente, ele apenas muda o valor somando mais o produto comprado
                    ComprasClientes[vendas.Cliente] = ComprasClientes[vendas.Cliente] + Produtos[vendas.Produto].Preco;
                else // se não existir ele adiciona mais um cliente pelo cpf
                    ComprasClientes.Add(vendas.Cliente, Produtos[vendas.Produto].Preco);
                #endregion

                #region Colhendo informações para o OpCodeH
                ProdutosVendidos[vendas.Produto] = ProdutosVendidos[vendas.Produto] + 1;
                #endregion

                #region Colhendo informações para o OpCodeJ
                dtaAux = FormataData(vendas.Data.Substring(0, 6));
                if (!VendasData.ContainsKey(dtaAux))
                    VendasData.Add(dtaAux, ArmazenaVendasData(dtaAux, Produtos[vendas.Produto].Preco));
                else
                    VendasData[dtaAux].Total += Produtos[vendas.Produto].Preco;
                #endregion

                #region Colhendo informações para o OpCode N

                if (vendcodigo == vendas.Codigo)
                {
                    // vendinfo.Cliente = Clientes[vendas.Cliente]; ---> Usar esse caso for o nome do cliente
                    vendinfo.Cliente = vendas.Cliente; //--->Usar esse caso for o CPF do cliente
                    vendinfo.Total += Produtos[vendas.Produto].Preco;
                }

                else
                {
                    if (Vendinha1.Count > 0 && vendinfo.Total > Vendinha1[0].Total)
                    {
                        Vendinha1.Clear();
                        Vendinha1.Add(vendinfo);
                    }

                    else if (vendinfo.Total == Vendinha1[0].Total)
                        Vendinha1.Add(vendinfo);


                    vendcodigo = vendas.Codigo;
                    vendinfo = ArmazenaVendasCod(vendcodigo);
                    vendinfo.Cliente = Clientes[vendas.Cliente];
                    vendinfo.Total += Produtos[vendas.Produto].Preco;
                }
                #endregion
            }
            File.AppendAllText("resultado.txt", "E- " + ProdutosVendidos.Count.ToString() + Environment.NewLine);
            Console.WriteLine("E: {0}", ProdutosVendidos.Count);
            #endregion

            #region F.Quantidade de nomes de clientes repetidos.
            int repetido = 0;
            List<string> aux = new List<string>();
            foreach (var dado in Clientes.Values)
            {
                if (!aux.Contains(dado))
                {
                    aux.Add(dado);
                    repetido++;
                }
            }
            File.AppendAllText("resultado.txt", "F- " + repetido.ToString() + Environment.NewLine);
            Console.WriteLine("F: " + repetido);
            #endregion

            List<string> resposta = new List<string>(); // usada no opCode G,P e Q

            #region G. Quantidade total de vendas por cliente (apenas dos clientes que efetuaram alguma compra), um por linha, ordenados pelo CPF. 

            foreach (var compra in ComprasClientes)
            {
                r.Append(compra.Key);
                r.Append("|");
                r.Append(Clientes[compra.Key]);
                r.Append("|");
                r.AppendLine(Math.Round(compra.Value, 2).ToString());
                resposta.Add(r.ToString());
                r.Clear();
            }
            resposta.Sort();           // ordena pelo cpf
            using (StreamWriter writer = new StreamWriter(@"resultado.txt", true))
            {
               for (int v = 0; v < resposta.Count; v++)
                {
                    writer.Write("G- " + resposta[v]);
                }
                writer.Close();
            }
            resposta = null;// para usar no proximo
            #endregion

            #region H. Quantidade e soma total de vendas por produto, um por linha, ordenado pelo nome do produto. 

            resposta = new List<string>();

            foreach (var k in ProdutosVendidos)
            {
                resposta.Add(Produtos[k.Key].Nome + "|" + k.Key + "|" + ProdutosVendidos[k.Key] + "|" + Math.Round(ProdutosVendidos[k.Key] * Produtos[k.Key].Preco, 2).ToString());
            }
            resposta.Sort();
            using (StreamWriter writer = new StreamWriter(@"resultado.txt", true))
            {
                for (int v = 0; v < resposta.Count; v++)
                {
                    r.Append("H- ");
                    r.AppendLine(resposta[v]);
                    //writer.Write("H- " + v + Environment.NewLine);
                    writer.Write(r.ToString());
                    r.Clear();
                }

                writer.Close();
            }
            resposta = null;// para usar no proximo  

            #endregion

            #region I.Valor (R$) total de vendas por categoria, uma por linha, ordenado pela descrição da categoria. 
            var comparador = new ComparadorValor();
            var resp = new List<CategoriaAuxiliar>();
            Dictionary<int, double> VendasPorCat = new Dictionary<int, double>();
            foreach (var g in Categorias.Keys)
                VendasPorCat.Add(g, 0);
            foreach (var t in vendas2)
                VendasPorCat[Produtos[t.Produto].Categoria] = VendasPorCat[Produtos[t.Produto].Categoria] + Produtos[t.Produto].Preco;
            foreach (var k in VendasPorCat.Keys)
            {
                resp.Add(AuxiliarCat(Categorias[k], Math.Round(VendasPorCat[k], 2), k));
            }
            resp.Sort(comparador);
            using (StreamWriter writer = new StreamWriter(@"resultado.txt", true))
            {
                foreach (var v in resp)
                {
                    writer.Write("I- " + v.Descricao + "|" + v.Codigo + "|" + v.Total + Environment.NewLine);
                }
            }
            #endregion

            #region J.Valor (R$) total de vendas por mês/ano, um por linha, ordenados por mês/ano            
            Stack<VendasPorData> pilhaVendasData = new Stack<VendasPorData>();
            var compData = new ComparadorData();
            VendasPorData valor;  
            VendasData.Values.ToList().Sort(compData);
            foreach (var vndt in VendasData.Values)
                pilhaVendasData.Push(vndt);
            using (StreamWriter writer = new StreamWriter(@"resultado.txt", true))
            {
                while (pilhaVendasData.Count != 0)
                {
                    valor = pilhaVendasData.Pop();
                    writer.Write("J- " + valor.Data.ToString("MM-yyyy") + '|' + Math.Round(valor.Total, 2) + Environment.NewLine);
                }
            }

            #endregion

            #region K. O cliente que mais comprou (R$)
            double maior = 0;
            string nome = "";
            string nome2 = "";
            foreach (var chave in ComprasClientes.Keys)
            {
                if (ComprasClientes[chave] > maior)
                {
                    maior = ComprasClientes[chave];
                    nome = Clientes[chave];
                }
            }
            foreach (string l in ComprasClientes.Keys)
                if (ComprasClientes[l] == maior)
                {
                    nome2 = Clientes[l];
                    File.AppendAllText("resultado.txt", "K- " + nome2 + '|' + maior + Environment.NewLine);
                }
            #endregion

            #region L.O produto mais vendido (quantidade de vendas) e a soma de suas vendas.
            maior = 0;
            foreach (var k in ProdutosVendidos.Keys)
                if (ProdutosVendidos[k] > maior)
                {
                    maior = ProdutosVendidos[k];
                }
            foreach (var n in ProdutosVendidos.Keys)
            {
                if (ProdutosVendidos[n] == maior)
                    // File.AppendAllText("resultado.txt", "L- " + Produtos[n].Nome + '|' + ProdutosVendidos[n]+'|'+ Math.Round(ProdutosVendidos[n] * Produtos[n].Preco, 2) + Environment.NewLine);
                    File.AppendAllText("resultado.txt", "L- " + Produtos[n].Nome + '|' + Math.Round(ProdutosVendidos[n] * Produtos[n].Preco, 2) + Environment.NewLine);
            }
            #endregion

            #region M.O mês/ano que mais que mais houve vendas (em valores R$).      
            ComparadorValorData c = new ComparadorValorData();
            VendasData.Values.ToList().Sort(c);
            maior = 0;
            foreach (var chave in VendasData.Values)
            {
                if (chave.Total > maior)
                {
                    maior = chave.Total;
                }
            }
            foreach (var l in VendasData.Values)
                if (l.Total == maior)
                {
                    File.AppendAllText("resultado.txt", "M- " + l.Data.ToString("MM-yyyy") + '|' + Math.Round(maior, 2) + Environment.NewLine);
                }
            #endregion

            #region N.A venda com valor mais alto.  

            using (StreamWriter writer = new StreamWriter(@"resultado.txt", true))
            {
                for (int v = 0; v < Vendinha1.Count; v++)
                {
                    r.Append("N - ");
                    r.Append(Vendinha1[v].Id);
                    r.Append("|");
                    r.Append(Vendinha1[v].Cliente);
                    r.Append("|");
                    r.AppendLine(Math.Round(Vendinha1[v].Total, 2).ToString());
                    writer.Write(r.ToString());
                    r.Clear();
                }
                writer.Close();
            }

            #endregion

            #region O. Produtos que não constam em nenhuma venda
            resposta = new List<string>();
            foreach (var prod in Produtos.Keys)
                if (!ProdutosVendidos.ContainsKey(prod))
                    resposta.Add(Produtos[prod].Nome);

            File.AppendAllText("resultado.txt", "O- " + resposta.Count + Environment.NewLine);

            resposta = null;
            #endregion

            #region P. Os clientes que não constam em nenhuma venda
            resposta = new List<string>();
            foreach (var v in Clientes.Keys)
            {
                if (!ComprasClientes.ContainsKey(v))
                    resposta.Add(v + '|' + Clientes[v]);
            }

            File.AppendAllText("resultado.txt", "P- " + resposta.Count + Environment.NewLine);

            resposta = null; // para usar no proximo
            #endregion

            #region Q.As categorias que não possuem produtos vendidos.
            resposta = new List<string>();
            bool tem = false;
            foreach (var cat in Categorias.Keys)
            {
                tem = false;
                foreach (var i in ProdutosVendidos.Keys)
                    if (Produtos[i].Categoria == cat)
                        tem = true;
                if (tem == false)
                    resposta.Add(Categorias[cat]);
            }
            File.AppendAllText("resultado.txt", "Q- " + resposta.Count + Environment.NewLine);
            #endregion


            TimeSpan tempo = DateTime.Now.Subtract(inicio); // calcula o total de segundos que demorou o processo
            File.AppendAllText("resultado.txt", DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine);
            double tempoSeg = Convert.ToDouble(Math.Round(tempo.TotalSeconds, 2));
            File.AppendAllText("resultado.txt", tempoSeg.ToString());
            Console.WriteLine(tempo.TotalSeconds);
            Console.WriteLine(tempo.TotalMinutes);
            Console.ReadLine();

        }
    }
}
