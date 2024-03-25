using ConsCCon.core;
using ConsCCon.core.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            //var sc = new ServicoConsulta { CNPJ = "12345678901234" };
            //var arq = "C:\\Users\\rechv\\git\\ConsCCon\\src\\Python\\cnpjs.csv";
            //Console.WriteLine($"Lendo {arq}.");
            //// sc.GeraTxtConsulta("PR", cfg.PastaArquivoCSV);
            //if (sc.ProcessaArqTxtBaseCnpj(arq, cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe))
            //    Console.WriteLine("Leu arquivo com sucesso.");

            // Leitura do arquivo XML
            var xmlArq = "C:\\Users\\rechv\\OneDrive\\Documentos\\[02] Profissional\\Projeto NFCom\\2024-03-21-Exemplo_cons_cad.xml";
            var sc = new ServicoConsulta();

            // Prepara dictionary
            var dic = new Dictionary<string, string>();

            cfg.ListaTagsRetornoXml.ToList().ForEach(obj => dic.Add(obj, ""));

            if (sc.LeXml(xmlArq, ref dic))
            {
                var csvArq = Path.Combine(cfg.PastaArquivoCSV, "arq.csv");

                if (sc.GravaCSVSaida(dic, cfg)) Utils.RegistraLogApp($"INFO: Gravou arquivo arquivo csv com sucesso.");
            }

            Console.Write("Pressione ENTER");
            Console.ReadLine();
        }
    }
}
