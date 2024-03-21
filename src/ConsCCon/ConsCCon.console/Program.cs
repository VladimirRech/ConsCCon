using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsCCon.core;
using ConsCCon.core.Classes;

namespace ConsCCon.console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lendo configurações");
            Configuracao cfg = Configuracao.LeConfiguracoes();

            if (!cfg.ValidaConfiguracao())
            {
                Console.WriteLine($"Configurações inválidas: {cfg.UltimaMsgErro}");
                return;
            }

            var sc = new ServicoConsulta { CNPJ = "12345678901234" };
            sc.GeraTxtConsulta("PR", cfg.PastaArquivoCSV);
        }
    }
}
