using System.Collections.Generic;

namespace TFX.Journal.DBJournal;

public partial class Campos
{
    public int idCampos
    {
        get; set;
    }

    public int idTabelas
    {
        get; set;
    }

    public string Nome
    {
        get; set;
    }

    public string Type
    {
        get; set;
    }

    public bool State
    {
        get; set;
    }

    public int Length
    {
        get; set;
    }

    public int Scale
    {
        get; set;
    }

    public bool IsPK
    {
        get; set;
    }

    public virtual ICollection<Revisoes> Revisoes { get; set; } = new List<Revisoes>();

    public virtual Tabelas idTabelasNavigation
    {
        get; set;
    }
}
