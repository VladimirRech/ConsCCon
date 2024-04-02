using ConsCCon.core.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsCCon.core
{
    public class ProcessaRetorno : BaseLog
    {
        string _pastaRetorno;
        string _pastaProcessados;
        List<string> _lstArqsRet;
        string _padraoArqRet;

        public bool ProcessarPasta(Configuracao cfg)
        {
            _pastaProcessados = cfg.PastaArquivosLidos;
            _pastaRetorno = cfg.PastaRetornoUninfe;
            _padraoArqRet = cfg.PadraoArquivoRet;

            if (!lePastaRetorno())
                return false;

            if (_lstArqsRet.Count == 0)
            {
                UltimaMsgErro = $"INFO: ProcessaRetorno - Nenhum arquivo encontrado na pasta {_pastaRetorno}";
                return false;
            }

            try
            {
                var sc = new ServicoConsulta();

                foreach (string arq in _lstArqsRet)
                {
                    var dic = new Dictionary<string, string>();
                    // preenche tags para serem lidas.
                    cfg.ListaTagsRetornoXml.ToList().ForEach(obj => dic.Add(obj, ""));

                    if (sc.LeXml(arq, ref dic))
                    {
                        if (sc.GravaCSVSaida(dic, cfg))
                        {
                            var arqDest = Path.Combine(_pastaProcessados, Path.GetFileName(arq));

                            try
                            {
                                File.Copy(arq, arqDest, true);
                                File.Delete(arqDest);
                            }
                            catch (Exception ex)
                            {
                                StackErro = ex.StackTrace;
                                UltimaMsgErro = $"ERRO: ProcessaRetorno - erro movendo arquivos de retorno. {ex.Message}";
                            }
                        }
                        else
                        {
                            UltimaMsgErro = $"ATENÇÃO: ProcessaRetorno - Não foi possível processar o arquivo {arq}.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackErro = ex.StackTrace;
                UltimaMsgErro = $"ERRO: ProcessaRetorno - erro lendo arquivos de retorno. {ex.Message}";
                return false;
            }

            return true;
        }

        private bool lePastaRetorno()
        {
            try
            {
                _lstArqsRet = new List<string>();
                _lstArqsRet.AddRange(Directory.GetFiles(_pastaRetorno, _padraoArqRet));
                return true;
            }
            catch (Exception ex)
            {
                StackErro = ex.StackTrace;
                UltimaMsgErro = ex.Message;
                return false;
            }
        }
    }
}
