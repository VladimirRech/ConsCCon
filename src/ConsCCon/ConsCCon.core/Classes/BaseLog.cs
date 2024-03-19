using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsCCon.core
{
    /// <summary>
    /// Estrutura base para as classes
    /// </summary>
    public class BaseLog
    {
        public string UltimaMsgErro { get; set; }
        public DateTime UltimoTimeStampErro { get; set; }
        public string StackErro { get; set; }

        public void CapturaErro(Exception ex)
        {
            UltimaMsgErro = ex.Message;
            UltimoTimeStampErro = DateTime.Now;
            StackErro = ex.StackTrace;
        }
    }
}
