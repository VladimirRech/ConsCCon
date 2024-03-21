using System.IO;
using System.Text;

namespace ConsCCon.core.Classes
{
    public class ServicoConsulta : BaseLog
    {
        public string CNPJ { get; set; }
        public string xServ { get { return "CONS-CAD"; } }
        public string Versao { get { return "2.00"; } }

        public bool GeraTxtConsulta(string UF, string pastaArquivo)
        {
            if (string.IsNullOrEmpty(CNPJ) || CNPJ.Length != 14)
            {
                UltimaMsgErro = $"ATENÇÃO: ServiçoConsulta - CNPJ inválido: {CNPJ}.";
                return false;
            }

            if (string.IsNullOrEmpty(UF))
            {
                UltimaMsgErro = "ATENÇÃO: ServicoConsulta - UF informada em branco.";
                return false;
            }

            var nomeArquivo = Path.Combine(pastaArquivo, $"{CNPJ}-cons-cad.txt");
            var sb = new StringBuilder();
            sb.AppendLine($"xServ|{xServ}");
            sb.AppendLine($"UF|{UF}");
            sb.AppendLine($"CNPJ|{CNPJ}"); 
            sb.AppendLine($"Versao|{Versao}");
            return Utils.GravaArquivo(sb.ToString(), nomeArquivo, false);
        }
    }
}
