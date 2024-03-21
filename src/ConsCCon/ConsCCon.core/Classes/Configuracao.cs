using System;
using System.Configuration;
using System.Text;

namespace ConsCCon.core
{
    public class Configuracao : BaseLog
    {
        public string CNPJCliente { get; set; }
        public string UFCliente { get; set; }
        public string PastaEnvioUninfe { get; set; }
        public string PastaRetornoUninfe { get; set; }
        public string PastaArquivoCSV { get; set; }
        public int ColunaCnpj { get; set; }
        public int ColunaUF { get; set; }

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
                ColunaUF = Convert.ToInt32(ConfigurationManager.AppSettings["ColunaUF"]?.ToString())
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

            if (sb.Length > 0)
            {
                StackErro = "";
                UltimaMsgErro = "ERRO: Verifique o arquivo de configurações. " + sb.ToString();
                return false;
            }

            return true;
        }
    }
}