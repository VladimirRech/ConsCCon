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
        public int LinhaInicialBaseCNPJ { get; set; }
        public int ColunaInicialBaseCnpj { get; set; }

        public static Configuracao LeConfiguracoes()
        {
            return new Configuracao
            {
                CNPJCliente = ConfigurationManager.AppSettings["CNPJCliente"]?.ToString(),
                UFCliente = ConfigurationManager.AppSettings["UFCliente"]?.ToString(),
                PastaEnvioUninfe = ConfigurationManager.AppSettings["PastaEnvioUninfe"]?.ToString(),
                PastaRetornoUninfe = ConfigurationManager.AppSettings["PastaRetornoUninfe"]?.ToString(),
                PastaArquivoCSV = ConfigurationManager.AppSettings["PastaArquivoCSV"]?.ToString(),
                LinhaInicialBaseCNPJ = Convert.ToInt32(ConfigurationManager.AppSettings["LinhaInicialBaseCNPJ"]?.ToString()),
                ColunaInicialBaseCnpj = Convert.ToInt32(ConfigurationManager.AppSettings["ColunaInicialBaseCnpj"]?.ToString())
            };
        }

        public bool ValidaConfiguracao()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(CNPJCliente))
            {
                sb.AppendLine("Configurações: CNPJ do cliente inválido .");
            }

            if (string.IsNullOrEmpty(UFCliente))
            {
                sb.AppendLine("Configurações: UF do cliente inválida.");
            }

            if (string.IsNullOrEmpty(PastaEnvioUninfe))
            {
                sb.AppendLine("Configurações: Pasta de envio do UNINFE inválida.");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaEnvioUninfe))
                {
                    sb.AppendLine($"Configurações: a pasta {PastaEnvioUninfe} informada na chave PastaEnvioUninfe não existe.");
                }
            }

            if (string.IsNullOrEmpty(PastaRetornoUninfe))
            {
                sb.AppendLine("Configurações: Pasta de retorno do UNINFE inválida.");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaRetornoUninfe))
                {
                    sb.AppendLine($"Configurações: a pasta {PastaRetornoUninfe} informada na chave PastaRetornoUninfe não existe.");
                }
            }

            if (string.IsNullOrEmpty(PastaArquivoCSV))
            {
                sb.AppendLine("Configurações: Pasta de geração do arquivo CSV inválida.");
            }
            else
            {
                if (!System.IO.Directory.Exists(PastaArquivoCSV))
                {
                    sb.AppendLine($"Configurações: a pasta {PastaArquivoCSV} informada na chave PastaArquivoCSV não existe.");
                }
            }

            if (LinhaInicialBaseCNPJ < 1)
            {
                sb.AppendLine("Configurações: Linha inicial para leitura da base de CNPJ inválida.");
            }
            
            if (ColunaInicialBaseCnpj < 1)
            {
                sb.AppendLine("Configurações: Coluna inicial para leitura da base de CNPJ inválida.");
            }

            if (sb.Length > 0)
            {
                UltimaMsgErro = sb.ToString();
                UltimoTimeStampErro = DateTime.Now;
                StackErro = "";
                return false;
            }

            return true;
        }
    }
}