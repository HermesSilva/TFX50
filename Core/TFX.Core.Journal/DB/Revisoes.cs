using System;

namespace TFX.Journal.DBJournal;

public partial class Revisoes
{
    public long idRevisoes
    {
        get; set;
    }

    public int idTabelas
    {
        get; set;
    }

    public DateTime Data
    {
        get; set;
    }

    public int idUsuario
    {
        get; set;
    }

    public int idCliente
    {
        get; set;
    }

    public int idPKTabela
    {
        get; set;
    }

    public byte idAcao
    {
        get; set;
    }

    public Guid Transacao
    {
        get; set;
    }

    public string Maquina
    {
        get; set;
    }

    public string Programa
    {
        get; set;
    }

    public virtual Campos idPKTabelaNavigation
    {
        get; set;
    }

    public virtual Tabelas idTabelasNavigation
    {
        get; set;
    }
}
