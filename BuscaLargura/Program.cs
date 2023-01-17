using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaLargura
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = new DirectoryInfo("../../");
            string diretorioAtual = dir.FullName;

            string entrada = $@"{diretorioAtual}\Documentos\Entrada.txt";
            string saidaTabelaTempo = $@"{diretorioAtual}\Documentos\TabelaTempo.txt";
            string saidaListaVisitados = $@"{diretorioAtual}\Documentos\ListaVisitados.txt";
            string saidaArvore = $@"{diretorioAtual}\Documentos\Arvore.txt";

            Grafo grafo = new Grafo();

            LerArquivo(entrada, ref grafo);

            grafo.MostrarVertices();

            Console.Write("\n>>Por qual vertice deve começar? ");
            string verticeInicial = Console.ReadLine();
            Console.WriteLine("");

            try
            {
                using (FileStream fs1 = new FileStream(saidaTabelaTempo, FileMode.Create))
                using (FileStream fs2 = new FileStream(saidaListaVisitados, FileMode.Create))
                using (FileStream fs3 = new FileStream(saidaArvore, FileMode.Create))
                {
                    using (StreamWriter arquivoTabelaTempo = new StreamWriter(fs1))
                    using (StreamWriter arquivoListaVisitados = new StreamWriter(fs2))
                    using (StreamWriter arquivoArvore = new StreamWriter(fs3))
                    {
                        grafo.BuscaEmLargura(verticeInicial, arquivoListaVisitados);
                        grafo.MostrarOrdemVisitados(arquivoListaVisitados);
                        grafo.MostrarTabelaTempo(arquivoTabelaTempo);
                        grafo.MostrarArvore(arquivoArvore);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        public static void LerArquivo(string path, ref Grafo grafo)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] vertices = sr.ReadLine().Split(',');
                            grafo.AddArestas(vertices[0], vertices[1]);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
