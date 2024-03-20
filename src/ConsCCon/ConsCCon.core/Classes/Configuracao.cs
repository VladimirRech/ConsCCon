using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
            return false;
        }
    }
}