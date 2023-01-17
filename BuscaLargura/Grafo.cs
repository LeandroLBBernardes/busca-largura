using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaLargura
{
    class Vertice
    {
        public string Nome { get; set; }
        public int Largura { get; set; }
        public int Nivel { get; set; }
        public string VerticePai { get; set; }
        public List<Vertice> ListaAdjacencia { get; set; }

        public Vertice(string nome)
        {
            Nome = nome;
            ListaAdjacencia = new List<Vertice>();
        }
    }

    class Grafo
    {
        public Dictionary<string, Vertice> Vertices;
        public List<string> OrdemVisitados, ArestasArvore;
        public List<Vertice> Fila { get; set; }

        public Grafo()
        {
            Vertices = new Dictionary<string, Vertice>();
            OrdemVisitados = new List<string>();
            ArestasArvore = new List<string>();
            Fila = new List<Vertice>();
        }

        public void AddArestas(string verticeOrigem, string verticeDestino)
        {
            if (!Vertices.ContainsKey(verticeOrigem))
                Vertices.Add(verticeOrigem, new Vertice(verticeOrigem));
            if (!Vertices.ContainsKey(verticeDestino))
                Vertices.Add(verticeDestino, new Vertice(verticeDestino));

            Vertice vertice1 = Vertices[verticeOrigem], vertice2 = Vertices[verticeDestino];

            vertice1.ListaAdjacencia.Add(vertice2);
            vertice2.ListaAdjacencia.Add(vertice1);
        }

        public void BuscaEmLargura(string verticeInicio, StreamWriter sw)
        {
            int largura = 1;
            Vertice verticeIniciado = Vertices[verticeInicio];

            largura = PercorrerVertices(verticeIniciado, sw, largura);

            foreach (Vertice vertice in Vertices.Values)
            {
                if (vertice.Largura == 0)
                {
                    largura = PercorrerVertices(vertice, sw, largura);
                }
            }
        }

        public int PercorrerVertices(Vertice verticeInicio, StreamWriter sw, int larguraInicio)
        {
            int largura = larguraInicio;
            Vertice verticeIniciado = verticeInicio;

            Fila.Add(verticeIniciado);
            MostrarListaVertice(verticeIniciado, sw);

            while (Fila.Count > 0)
            {
                verticeIniciado = Fila.First();
                verticeIniciado.Largura = largura;

                OrdemVisitados.Add(verticeIniciado.Nome);

                largura++;
                foreach (Vertice vertice in verticeIniciado.ListaAdjacencia)
                {
                    if (vertice.Largura == 0)
                    {
                        Fila.Add(vertice);
                        MostrarListaVertice(vertice, sw);
                        vertice.Largura = largura;
                        vertice.Nivel = verticeIniciado.Nivel + 1;
                        vertice.VerticePai = verticeIniciado.Nome;
                        ArestasArvore.Add("{" + $"{verticeIniciado.Nome},{vertice.Nome}" + "}");
                    }
                }

                Fila.RemoveAt(0);
            }

            return largura;
        }

        public void MostrarVertices()
        {
            Console.WriteLine("Vertices: ");
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                Console.Write($"{vertice.Key} ");
            }
            Console.WriteLine($"\n\nTotal de Vertices: {Vertices.Count}");
        }

        public void MostrarListaVertice(Vertice vertice, StreamWriter sw)
        {
            Console.Write($"Y({vertice.Nome}) = " + "{ ");
            sw.Write($"Y({vertice.Nome}) = " + "{ ");

            foreach (Vertice verticeAux in vertice.ListaAdjacencia)
            {
                if (vertice.ListaAdjacencia.Last().Nome.Equals(verticeAux.Nome))
                {
                    Console.Write($"{verticeAux.Nome}");
                    sw.Write($"{verticeAux.Nome}");
                    break;
                }
                Console.Write($"{verticeAux.Nome}, ");
                sw.Write($"{verticeAux.Nome}, ");
            }

            Console.Write(" }\n");
            sw.Write(" }\n");
        }

        public void MostrarOrdemVisitados(StreamWriter sw)
        {
            Console.WriteLine("\nLista de visitados: ");
            sw.WriteLine("\nLista de visitados: ");
            foreach (string vertice in OrdemVisitados)
            {
                Console.Write($"{vertice} ");
                sw.Write($"{vertice} ");
            }
        }

        public void MostrarTabelaTempo(StreamWriter sw)
        {
            Console.WriteLine("\n\n|Vertice|L \t|N \t|Pai");
            sw.WriteLine("\n\n|Vertice \t|L \t|N \t|Pai");
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                string Nome = vertice.Value.Nome;
                int Largura = vertice.Value.Largura;
                int Nivel = vertice.Value.Nivel;
                string VerticePai = vertice.Value.VerticePai;

                Console.WriteLine($"|   {Nome}\t|{Largura}\t|{Nivel}\t|{VerticePai}");
                sw.WriteLine($"|   {Nome}\t|{Largura}\t|{Nivel}\t|{VerticePai}");
            }
        }

        public void MostrarArvore(StreamWriter sw)
        {
            Console.Write("\nE = {");
            sw.Write("\nE = {");
            foreach (string aresta in ArestasArvore)
            {
                Console.Write($" {aresta} ");
                sw.Write($" {aresta} ");
            }
            Console.Write("}");
            sw.Write("}");
        }
    }
}
