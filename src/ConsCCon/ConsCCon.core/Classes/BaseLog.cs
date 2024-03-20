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
        private string _ultimaMsgErro;

        public string UltimaMsgErro
        {
            get
            {
                return _ultimaMsgErro;
            }
            set
            {
                _ultimaMsgErro = value;
                Utils.RegistraLogApp($"{value} | {StackErro}");
            }
        }
        public string StackErro { get; set; }

        public void CapturaErro(Exception ex)
        {
            StackErro = ex.StackTrace;
            UltimaMsgErro = ex.Message;
        }
    }
}
