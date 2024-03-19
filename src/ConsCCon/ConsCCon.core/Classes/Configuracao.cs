using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return null;
        }

        public bool ValidaConfiguracao()
        {
            return false;
        }
    }
}
