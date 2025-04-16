namespace TFX.Journal.DBJournal;

public partial class Selects
{
    public int idSelects
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

    public string Query
    {
        get; set;
    }

    public virtual Tabelas idTabelasNavigation
    {
        get; set;
    }
}
