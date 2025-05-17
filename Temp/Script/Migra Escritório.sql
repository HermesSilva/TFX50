--create view Escritorio as SELECT  Cnpj, Nome FROM sittax..Escritorio


-- Tabela temporária para registrar os novos CORxPessoaID gerados
CREATE TABLE #PessoasGeradas (
    CORxPessoaID UNIQUEIDENTIFIER,
    Nome VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AI,
    CPFCNPJ VARCHAR(18) COLLATE SQL_Latin1_General_CP1_CI_AI
)

delete CORxAgregado where CORxAgregadoID<>'00000000-0000-0000-0000-000000000000'
delete CORxPessoa where CORxPessoaID not in ('00000000-0000-0000-0000-000000000000','E3D57815-06E9-46E0-96F2-D77A03700CA8')
delete #PessoasGeradas
-- Inserir apenas os escritórios que ainda não existem em CORxPessoa
INSERT INTO #PessoasGeradas (CORxPessoaID, Nome, CPFCNPJ)
SELECT 
    NEWID(),
    e.Nome COLLATE SQL_Latin1_General_CP1_CI_AI,
    e.Cnpj COLLATE SQL_Latin1_General_CP1_CI_AI
FROM Escritorio e

-- Inserir na tabela CORxPessoa
INSERT INTO CORxPessoa (CORxPessoaID, Nome)
SELECT CORxPessoaID, Nome
FROM #PessoasGeradas



-- Inserir na tabela CORxAgregado usando o mesmo ID
INSERT INTO CORxAgregado (CORxAgregadoID, CORxStatusID, CPFCNPJ)
SELECT CORxPessoaID, 1, CPFCNPJ
FROM #PessoasGeradas
-- Inserir na tabela ESCxEscritorio usando o mesmo ID

INSERT INTO ESCxEscritorio (ESCxEscritorioID)
SELECT CORxPessoaID
FROM #PessoasGeradas
-- Limpeza
-- DROP TABLE #PessoasGeradas
