# ConsCCon
Projeto privado de consulta ao cadastro de contribuintes.

## OBJETIVO
Criar uma aplicação para consulta do cadastro do contribuinte nos webservices SEFAZ/NF-e. 
A consulta será realizada usando o número do CNPJ do contribuinte. 
Os dados serão retornados no formato XML enviados pelos webservices do SEFAZ/NF-e e gravados em arquivo neste formato.

## ESCOPO DA APLICAÇÃO

Será criada uma aplicação console do Windows que receberá como parâmetro um número de CNPJ a ser consultado ou um arquivo texto contendo em suas linhas os números de CNPJ a serem consultados.
A aplicação console irá criar um arquivo texto com os parâmetros necessários para a consulta do cadastro junto ao SEFAZ/NF-e.
O arquivo será gravado em uma pasta compartilhada com a aplicação UNINFE que irá ler o arquivo, validar as informações e realizará a consulta ao webservices.
O UNINFE irá gerar um arquivo XML com o retorno da consulta em uma pasta configurada para esta finalidade. O arquivo tem o layout usado pelo Webservice de consulta, no formato XML.
A aplicação console não irá fazer o tratamento nem a leitura das informações retornadas pela consulta do webservice.
A aplicação console irá manter em suas configurações os seguintes dados para realizar a consulta:
  - CNPJ do emitente da NFE para consultar o cadastro.
  - UF do emitente
  - Pasta de envio dos arquivos de texto para consulta.
  - Layout do arquivo de texto contendo a relação de CNPJ para consulta. 


## REQUISITOS TÉCNICOS

- Microsoft DotNet Framework 4.8 (usado pela aplicação console e pelo UNINFE).
- UNINFE instalado e configurado.
- Certificado digital de emissor de NF-e autorizado.
- Endereços de webservices da NF-e liberados no firewall (no ambiente de produção).
- Permissões de leitura e escrita em pastas de rede (no ambiente de produção).
- Todas as aplicações são executadas apenas no Windows.
