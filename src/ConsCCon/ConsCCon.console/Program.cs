using ConsCCon.core;
using ConsCCon.core.Classes;
using System;
using System.Linq;

namespace ConsCCon.console
{
    internal class Program
    {
        static string[] helpArgs = new string[5] { "/?", "/h", "-h", "--help", "-?" };
        
        enum Modo
        {
            INDEFINIDO,
            CONSULTAR_CNPJ,
            CONSULTAR_ARQUIVO,
            LER_RETORNO,
            LER_ERROS
        }

        static Modo modoAtual;

        static void Main(string[] args)
        {
            if (!validaArgs(args)) return;

            Console.WriteLine("Lendo configurações.");
            Configuracao cfg = Configuracao.LeConfiguracoes();

            if (!cfg.ValidaConfiguracao())
            {
                Console.WriteLine($"Configurações inválidas: {cfg.UltimaMsgErro}");
                return;
            }

            switch (modoAtual)
            {
                case Modo.INDEFINIDO:
                    return;
                case Modo.CONSULTAR_CNPJ:
                    Console.WriteLine("Iniciando geração de XML para consulta de CNPJ.");

                    if (consultaCnpj(args[1], args[2], cfg.PastaEnvioUninfe))
                    {
                        Console.WriteLine($"Gerou dados da consulta do CNPJ {args[1]} para o Estado {args[2]}.");
                    }
                    break;
                case Modo.CONSULTAR_ARQUIVO:
                    Console.WriteLine("Iniciando geração de XML para consulta de CNPJ a partir de arquivo.");

                    if (new ServicoConsulta().ProcessaArqTxtBaseCnpj(args[1], cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe))
                    {
                        Console.WriteLine($"Gerou dados de consulta para o arquivo {args[1]}.");
                    }
                    break;
                case Modo.LER_RETORNO:
                    var pr = new ProcessaRetorno();
                    Console.WriteLine("Iniciando leitura de arquivos de retorno.");

                    if (pr.ProcessarPasta(cfg))
                    {
                        Console.WriteLine("Leu os arquivos de retorno com sucesso.");
                    }
                    break;
                case Modo.LER_ERROS:
                    var prErro = new ProcessaRetorno();
                    Console.WriteLine("Lendo arquivos de erro.");

                    if (prErro.ProcessaArquivosDeErro(cfg.PastaRetornoUninfe))
                    {
                        Console.WriteLine("Leu os arquivos de erro com sucesso.");
                    }
                    break;
                default:
                    break;
            }

            Console.WriteLine("Operação concluída.");

#if DEBUG
            Console.Write("Pressione ENTER");
            Console.ReadLine();
#endif
        }

        private static bool validaArgs(string[] args)
        {
            if (args.Length == 0 || helpArgs.Contains(args[0]))
            {
                Console.WriteLine("Número de argumentos inválidos.");
                Console.WriteLine("Uso:");
                Console.WriteLine("  consultar <cnpj> <uf> | <nome arquivo cnpjs>    Envia dados para consulta.");
                Console.WriteLine("  ler                                             Processa retorno da consulta.");
                Console.WriteLine("  erro                                            Processa arquivos de erro.");
                Console.WriteLine("                                                  Grava na pasta atual arquivo txt.");
                Console.WriteLine("  -? | /? | -h | --help | /h                      Mostra ajuda com argumentos.");
                return false;
            }

            var erro = "Parâmetros inválidos.";

            switch (args[0].ToLower())
            {
                case "consultar":
                    {
                        modoAtual = args.Length == 2 ? Modo.CONSULTAR_ARQUIVO : args.Length == 3 ? Modo.CONSULTAR_CNPJ : Modo.INDEFINIDO;
                        break;
                    }
                case "ler":
                    {
                        modoAtual = modoAtual = Modo.LER_RETORNO; 
                        break;
                    }
                case "erro":
                    {
                        modoAtual = Modo.LER_ERROS;
                        break;
                    }
            }

            erro = modoAtual != Modo.INDEFINIDO ? "" : erro;

            if (erro.Length == 0) return true;

            Console.WriteLine(erro);
            return false;
        }

        private static bool consultaCnpj(string cnpj, string uf, string pastaEnvioUninfe)
        {
            var sc = new ServicoConsulta { CNPJ = cnpj, UF = uf };
            return sc.GeraTxtConsulta(uf, pastaEnvioUninfe);
        }
    }
}
