using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsCCon.core
{
    public class Configuracao : BaseLog
    {
        const int MIN_SEGUNDOS_RETORNO = 30;
        const int MAX_SEGUNDOS_RETORNO = 600;
        const int MIN_REPETICOES_RETORNO = 1;
        const int MAX_REPETICOES_RETORNO = 10;

        public string CNPJCliente { get; set; }
        public string UFCliente { get; set; }
        public string PastaEnvioUninfe { get; set; }
        public string PastaRetornoUninfe { get; set; }
        public string PastaArquivoCSV { get; set; }
        public int ColunaCnpj { get; set; }
        public int ColunaUF { get; set; }

        private string _TagsRetornoXml;

        public string TagsRetornoXml
        {
            get { return _TagsRetornoXml; }
            set { _TagsRetornoXml = value; }
        }

        public IEnumerable<string> ListaTagsRetornoXml
        {
            get { return TagsRetornoXml.Split(';').ToList(); }
        }

        public string NomeArquivoCSV { get; set; }

        public Dictionary<string, string> PadraoArqCDSV { get; private set; }

        public int SegundosEsperaRetorno { get; set; }
        public int RepeticoesRetorno { get; set; }
        public string PastaArquivosLidos { get; set; }

        public Configuracao()
        {
            PadraoArqCDSV = new Dictionary<string, string>();
            PadraoArqCDSV.Add("DDMMYYYY", "ddMMyyyy");
            PadraoArqCDSV.Add("YYYYMMDD", "yyyyMMdd");
            PadraoArqCDSV.Add("YYYYMM", "yyyyMM");
            PadraoArqCDSV.Add("MMYYYY", "MMyyyy");
        }

        public static Configuracao LeConfiguracoes()
        {
            Utils.RegistraLogApp("INFO: Lendo configurações.");

            return new Configuracao
            {
                CNPJCliente = ConfigurationManager.AppSettings["CNPJCliente"]?.ToString(),
                UFCliente = ConfigurationManager.AppSettings["UFCliente"]?.ToString(),
                PastaEnvioUninfe = ConfigurationManager.AppSettings["PastaEnvioUninfe"]?.ToString(),
                PastaRetornoUninfe = ConfigurationManager.AppSettings["PastaRetornoUninfe"]?.ToString(),
                PastaArquivoCSV = ConfigurationManager.AppSettings["PastaArquivoCSV"]?.ToString(),
                ColunaCnpj = Convert.ToInt32(ConfigurationManager.AppSettings["ColunaCnpj"]?.ToString()),
                ColunaUF = Convert.ToInt32(ConfigurationManager.AppSettings["ColunaUF"]?.ToString()),
                TagsRetornoXml = ConfigurationManager.AppSettings["TagsRetornoXml"]?.ToString(),
                NomeArquivoCSV = ConfigurationManager.AppSettings["NomeArquivoCSV"]?.ToString(),
                SegundosEsperaRetorno = Convert.ToInt32(ConfigurationManager.AppSettings["SegundosEsperaRetorno"]?.ToString()),
                RepeticoesRetorno = Convert.ToInt32(ConfigurationManager.AppSettings["RepeticoesRetorno"]?.ToString()),
                PastaArquivosLidos = ConfigurationManager.AppSettings["PastaArquivosLidos"]?.ToString()
            };
        }

        public bool ValidaConfiguracao()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(CNPJCliente))
            {
                sb.Append("Configurações: CNPJ do cliente inválido. ");
            }

            if (string.IsNullOrEmpty(UFCliente))
            {
                sb.Append("Configurações: UF do cliente inválida. ");
            }

            if (string.IsNullOrEmpty(PastaEnvioUninfe))
            {
                sb.Append("Configurações: Pasta de envio do UNINFE inválida. ");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaEnvioUninfe))
                {
                    sb.Append($"Configurações: a pasta {PastaEnvioUninfe} informada na chave PastaEnvioUninfe não existe. ");
                }
            }

            if (string.IsNullOrEmpty(PastaRetornoUninfe))
            {
                sb.Append("Configurações: Pasta de retorno do UNINFE inválida. ");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaRetornoUninfe))
                {
                    sb.Append($"Configurações: a pasta {PastaRetornoUninfe} informada na chave PastaRetornoUninfe não existe. ");
                }
            }

            if (string.IsNullOrEmpty(PastaArquivoCSV))
            {
                sb.Append("Configurações: Pasta de geração do arquivo CSV inválida. ");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaArquivoCSV))
                {
                    sb.Append($"Configurações: a pasta {PastaArquivoCSV} informada na chave PastaArquivoCSV não existe. ");
                }
            }

            if (ColunaCnpj < 1)
            {
                sb.Append("Configurações: Coluna do CNPJ inválida. ");
            } 
            
            if (ColunaUF < 1)
            {
                sb.Append("Configurações: Coluna da UF inválida. ");
            }

            if (string.IsNullOrEmpty(NomeArquivoCSV))
            {
                sb.Append("Configuração do nome do arquivo CSV inválida.");
            }
            else
            {
                if (!ValidaNomeArquivo(NomeArquivoCSV))
                {
                    sb.Append($"Verifique a configuração NomeArquivoCSV, o conteúdo é inválido: {NomeArquivoCSV} ");
                }
            }

            if (SegundosEsperaRetorno < MIN_SEGUNDOS_RETORNO || SegundosEsperaRetorno > MAX_SEGUNDOS_RETORNO)
            {
                sb.Append("Tempo de espera entre as leituras de retorno da consulta é inválido, verifique a configuração SegundosEsperaRetorno. ");
            }

            if (RepeticoesRetorno < MIN_REPETICOES_RETORNO || RepeticoesRetorno > MAX_REPETICOES_RETORNO)
            {
                sb.Append("Número de repetições das leituras de retorno da consulta é inválido, verifique a configuração RepeticoesRetorno. ");
            }

            if (string.IsNullOrEmpty(PastaArquivosLidos))
            {
                sb.Append("Configurações: Pasta para armazenar retornos lidos inválida, verifique a configuração PastaArquivosLidos. ");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaArquivosLidos))
                {
                    sb.Append($"Configurações: a pasta {PastaArquivosLidos} informada na chave PastaArquivosLidos não existe. ");
                }
            }

            if (sb.Length > 0)
            {
                StackErro = "";
                UltimaMsgErro = "ERRO: Verifique o arquivo de configurações. " + sb.ToString();
                return false;
            }

            return true;
        }

        private bool ValidaNomeArquivo(string padrao)
        {
            var regex = new Regex("^([^\\/:\\*\\?\"<>\\|]+)$");
            return regex.IsMatch(padrao);
        }
    }
}