﻿using ConsCCon.core;
using ConsCCon.core.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
            LER_RETORNO
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
                    if (consultaCnpj(args[1], args[2], cfg.PastaEnvioUninfe))
                    {
                        Console.WriteLine($"Gerou dados da consulta do CNPJ {args[1]} para o Estado {args[2]}.");
                    }
                    break;
                case Modo.CONSULTAR_ARQUIVO:
                    if (new ServicoConsulta().ProcessaArqTxtBaseCnpj(args[1], cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe))
                    {
                        Console.WriteLine($"Gerou dados de consulta para o arquivo {args[1]}.");
                    }
                    break;
                case Modo.LER_RETORNO:
                    break;
                default:
                    break;
            }

            Console.Write("Pressione ENTER");
            Console.ReadLine();
        }

        private static bool validaArgs(string[] args)
        {
            if (args.Length == 0 || helpArgs.Contains(args[0]))
            {
                Console.WriteLine("Número de argumentos inválidos.");
                Console.WriteLine("Uso:");
                Console.WriteLine("  consultar <cnpj> <uf> | <nome arquivo cnpjs>    Envia dados para consulta.");
                Console.WriteLine("  ler                                             Processa retorno da consulta.");
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

        private static bool consultaCnpj(string arq, Configuracao cfg)
        {
            return new ServicoConsulta().ProcessaArqTxtBaseCnpj(arq, cfg.ColunaCnpj, cfg.ColunaUF, cfg.PastaEnvioUninfe);
        }
    }
}
