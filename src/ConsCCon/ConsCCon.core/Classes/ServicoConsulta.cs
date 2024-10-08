﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ConsCCon.core.Classes
{
    public class ServicoConsulta : BaseLog
    {
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string xServ { get { return "CONS-CAD"; } }
        public string Versao { get { return "2.00"; } }
        private int _tamCnpj;
        private int _tamIE;

        public ServicoConsulta() { }

        public ServicoConsulta(int tamCnpj, int tamIE)
        {
            _tamCnpj = tamCnpj;
            _tamIE = tamIE;
        }

        public bool GeraTxtConsulta(string uf, string pastaArquivo)
        {
            if (string.IsNullOrEmpty(CNPJ) || CNPJ.Length != 14)
            {
                UltimaMsgErro = $"ATENÇÃO: ServiçoConsulta - CNPJ inválido: {CNPJ}.";
                return false;
            }

            UF = uf;

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

        public bool ProcessaArqTxtBaseCnpj(string arquivo, int colunaCnpj, int colunaUf, string pastaArquivo)
        {
            Utils.RegistraLogApp($"INFO: ServicoConsulta - Iniciando leitura do arquivo {arquivo}.");

            if (!File.Exists(arquivo))
            {
                UltimaMsgErro = $"ERRO: ServicoConsulta - O arquivo {arquivo} informado não existe.";
                return false;
            }

            // Tratamento para as configurações de primeira linha e coluna
            colunaCnpj--;
            colunaUf--;
            int contador = 0;

            try
            {
                using (StreamReader sr = new StreamReader(arquivo))
                {
                    string linha = "";

                    while ((linha = sr.ReadLine()) != null)
                    {
                        if (contador == 0)
                        {
                            contador++;
                            continue;
                        }

                        if (linha.Length < 17)
                        {
                            UltimaMsgErro = $"ATENÇÃO: ServicoConsulta - a linha nr. {contador} do arquivo {arquivo} tem um tamanho inválido.";
                            contador++;
                            continue;
                        }

                        var arrRegistro = linha.Split(';');

                        if (arrRegistro.Length < colunaUf + 1)
                        {
                            throw new Exception($"Layout do arquivo {arquivo} incorreto.");
                        }

                        CNPJ = arrRegistro[colunaCnpj];

                        if (CNPJ.Length != 14)
                        {
                            UltimaMsgErro = $"ATENÇÃO: ServicoConsulta - o CNPJ da linha nr. {contador} do arquivo {arquivo} tem um tamanho inválido.";
                            contador++;
                            continue;
                        }

                        UF = arrRegistro[colunaUf];

                        if (UF.Length != 2)
                        {
                            UltimaMsgErro = $"ATENÇÃO: ServicoConsulta - a UF da linha nr. {contador} do arquivo {arquivo} tem um tamanho inválido.";
                            contador++;
                            continue;
                        }

                        GeraTxtConsulta(UF, pastaArquivo);
                        contador++;
                    }
                }

                Utils.RegistraLogApp($"INFO: ServicoConsulta - Terminou leitura do arquivo {arquivo}.");
                return true;
            }
            catch (Exception ex)
            {
                CapturaErro(ex);
                return false;
            }
        }

        public bool LeXml(string arquivo, ref Dictionary<string, string> dict, Configuracao config)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(arquivo);
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("infCad");

            if (nodes.Count == 0)
            {
                nodes = xmlDoc.GetElementsByTagName("infCons");
            }

            foreach (XmlNode node in nodes)
            {
                processaChildrenNodes(node, ref dict);

                if (!GravaCSVSaida(dict, config))
                {
                    StackErro = "";
                    UltimaMsgErro = $"ATENÇÃO: ServicoConsulta.LeXml - Não foi possível ler o arquivo {arquivo}";
                    return false;
                }
            }

            return true;
        }

        public bool GravaCSVSaida(Dictionary<string, string> dict, Configuracao config)
        {
            try
            {
                string arquivo = config.NomeArquivoCSV;

                foreach (string key in config.PadraoArqCDSV.Keys)
                {
                    if (config.NomeArquivoCSV.Contains(key))
                    {
                        arquivo = config.NomeArquivoCSV.Replace($"{key}", $"{DateTime.Today.ToString($"{config.PadraoArqCDSV[key]}")}");
                        break;
                    }
                }

                var sbCab = new StringBuilder();
                var sbLinha = new StringBuilder();
                arquivo = Path.Combine(config.PastaArquivoCSV, arquivo);

                foreach (string key in dict.Keys)
                {
                    sbCab.Append(key + ";");

                    if (key.ToLower() == "cnpj")
                    {
                        sbLinha.Append(dict[key].PadLeft(_tamCnpj, '0') + ";");
                        continue;
                    }

                    if (key.ToLower() == "ie")
                    {
                        sbLinha.Append(dict[key].PadLeft(_tamIE, '0') + ";");
                        continue;
                    }

                    sbLinha.Append(dict[key] + ";");
                }

                if (!File.Exists(arquivo))
                {
                    Utils.GravaArquivo(sbCab.ToString().Substring(0, sbCab.ToString().Length - 1) + "\r\n", arquivo, false);
                }

                Utils.GravaArquivo(sbLinha.ToString().Substring(0, sbLinha.ToString().Length - 1) + "\r\n", arquivo, true);
                return true;
            }
            catch (Exception ex)
            {
                CapturaErro(ex);
                return false;
            }
        }

        private void ProcessaNoXml(XmlNode noXml, ref Dictionary<string, string> dict)
        {
            if (noXml.HasChildNodes)
            {
                foreach (XmlNode child in noXml.ChildNodes) ProcessaNoXml(child, ref dict);
            }
            else
            {
                if (dict.ContainsKey(noXml.ParentNode.Name)) dict[noXml.ParentNode.Name] = noXml.InnerText;
            }
        }

        private void processaChildrenNodes(XmlNode node, ref Dictionary<string, string> dic)
        {
            if (node.NodeType == XmlNodeType.Element && node.ChildNodes.Count > 1)
                foreach (XmlNode child in node.ChildNodes)
                {
                    processaChildrenNodes(child, ref dic);
                }
            else
            {
                if (dic.ContainsKey(node.Name))
                    dic[node.Name] = node.InnerText;
            }
        }
    }
}
