using System;
using System.IO;
using System.Text;

namespace ConsCCon.core
{
    internal class Utils
    {
        internal static void RegistraLogApp(string texto)
        {
            var nomeArqLog = string.Format("CONSCCon_{0:yyyyMMdd}.log", DateTime.Now);
            GravaArquivo(string.Format("{0:yyyy-MM-dd HH:mm:ss}: {1}{2}", DateTime.Now, texto, Environment.NewLine), nomeArqLog, true);
        }

        internal static bool GravaArquivo(string texto, string nomeArquivo, bool adiciona)
        {
            try
            {
                if (!File.Exists(nomeArquivo) || !adiciona)
                {
                    File.WriteAllText(nomeArquivo, texto, Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(nomeArquivo, texto, Encoding.UTF8);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GravarArquivo: Falha:\r\n\t{ex.Message}");
                return false;
            }
        }
    }
}
