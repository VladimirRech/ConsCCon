using ConsCCon.core;
using ConsCCon.core.Classes;
using System;

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
            var arq = "C:\\Users\\rechv\\git\\ConsCCon\\src\\Python\\cnpjs.csv";
            Console.WriteLine($"Lendo {arq}.");
            // sc.GeraTxtConsulta("PR", cfg.PastaArquivoCSV);
            if (sc.ProcessaArqTxtBaseCnpj(arq, cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe))
                Console.WriteLine("Leu arquivo com sucesso.");
        }
    }
}
