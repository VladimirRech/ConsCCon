using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsCCon.core;

namespace ConsCCon.console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lendo configurações");
            Configuracao cfg = Configuracao.LeConfiguracoes();

            if (!cfg.ValidaConfiguracao())
            {
                Console.WriteLine($"Configurações inválidas: {cfg.UltimaMsgErro}");
            }
            else
            {
                Console.WriteLine("Configurações válidas.");
            }
        }
    }
}
