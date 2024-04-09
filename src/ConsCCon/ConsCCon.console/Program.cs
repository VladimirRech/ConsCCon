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
            LER_ERROS,
            DEBUG_MODE
        }

        static Modo modoAtual;

        static void Main(string[] args)
        {
            if (!validaArgs(args)) return;

            EscreveLog("Lendo configurações.", true);
            Configuracao cfg = Configuracao.LeConfiguracoes();

            if (!cfg.ValidaConfiguracao())
            {
                EscreveLog($"Configurações inválidas: {cfg.UltimaMsgErro}", true);
                return;
            }

            switch (modoAtual)
            {
                case Modo.INDEFINIDO:
                    return;
                case Modo.CONSULTAR_CNPJ:
                    EscreveLog("Iniciando geração de XML para consulta de CNPJ.", true);

                    if (consultaCnpj(args[1], args[2], cfg.PastaEnvioUninfe))
                    {
                        EscreveLog($"Gerou dados da consulta do CNPJ {args[1]} para o Estado {args[2]}.", true);
                    }
                    break;
                case Modo.CONSULTAR_ARQUIVO:
                    EscreveLog("Iniciando geração de XML para consulta de CNPJ a partir de arquivo.", true);

                    if (new ServicoConsulta().ProcessaArqTxtBaseCnpj(args[1], cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe))
                    {
                        EscreveLog($"Gerou dados de consulta para o arquivo {args[1]}.", true);
                    }
                    break;
                case Modo.LER_RETORNO:
                    var pr = new ProcessaRetorno();
                    EscreveLog("Iniciando leitura de arquivos de retorno.", true);

                    if (pr.ProcessarPasta(cfg))
                    {
                        EscreveLog("Leu os arquivos de retorno com sucesso.", true);
                    }
                    break;
                case Modo.LER_ERROS:
                    var prErro = new ProcessaRetorno();
                    EscreveLog("Lendo arquivos de erro.", true);

                    if (prErro.ProcessaArquivosDeErro(cfg.PastaRetornoUninfe))
                    {
                        EscreveLog("Leu os arquivos de erro com sucesso.", true);
                    }
                    break;
                case Modo.DEBUG_MODE:
                    {
                        // código de teste
                        break;
                    }
                default:
                    break;
            }

            EscreveLog("Operação concluída.", true);

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
                case "debug":
                    {
                        modoAtual = Modo.DEBUG_MODE;
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

        private static void EscreveLog(string msg, bool escreveConsole)
        {
            Utils.RegistraLogApp(msg);

            if (escreveConsole)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
