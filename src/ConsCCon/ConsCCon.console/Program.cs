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
            Console.WriteLine("Lendo C:\\Users\\rechv\\git\\ConsCCon\\src\\Python\\new_file.csv");
            // sc.GeraTxtConsulta("PR", cfg.PastaArquivoCSV);
            if (sc.ProcessaArqTxtBaseCnpj("C:\\Users\\rechv\\git\\ConsCCon\\src\\Python\\new_file.csv", cfg.LinhaInicialBaseCNPJ, cfg.ColunaInicialBaseCnpj, "PR", cfg.PastaEnvioUninfe))
                Console.WriteLine("Leu arquivo com sucesso.");
        }
    }
}
