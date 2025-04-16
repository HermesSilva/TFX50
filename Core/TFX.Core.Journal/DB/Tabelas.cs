using System.Collections.Generic;

namespace TFX.Journal.DBJournal;

public partial class Tabelas
{
    public int idTabelas
    {
        get; set;
    }

    public string Nome
    {
        get; set;
    }

    public string Esquema
    {
        get; set;
    }

    public string CampoPK
    {
        get; set;
    }

    public virtual ICollection<Campos> Campos { get; set; } = new List<Campos>();

    public virtual ICollection<Revisoes> Revisoes { get; set; } = new List<Revisoes>();

    public virtual ICollection<Selects> Selects { get; set; } = new List<Selects>();
}
