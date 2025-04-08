/// <reference path="../Elements/Base/XBaseButtonInput.ts" />
class XDatePickerEditor extends XBaseLoockupInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Calendar = <XCalendar>this.DropDownContent;
        this.Input.className = "XDatePickerEditor";
        this.Calendar.IsVisible = false;
        this.Calendar.OnSelectdate = (d) => this.Selected(d);
        this.Calendar.ReferenceElement = this;
        this.Input.placeholder = 'dd/mm/aaaa';
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.HandleInput);
        this.Title = "Digite uma Data";

    }
    Calendar: XCalendar ;
    SelectedDate: Date = new Date();

    protected CreateDropDown(): XDropDownElement
    {
        this.Calendar = new XCalendar(<any>this.Owner, "XCalendar");
        return this.Calendar;
    }

    Selected(pDate: Date)
    {
        this.SelectedDate = pDate
        this.Calendar.IsVisible = false;
        this.Input.value = this.FormatDate(pDate, this.Input.placeholder);
    }

    FormatDate(data: Date, formato: string): string
    {
        const completarComZero = (valor: number): string =>
        {
            return valor < 10 ? `0${valor}` : valor.toString();
        };

        // Extrai componentes da data
        const dia = completarComZero(data.getDate());
        const mes = completarComZero(data.getMonth() + 1); // +1 pois meses são 0-based
        const ano = data.getFullYear().toString();
        const horas = completarComZero(data.getHours());
        const minutos = completarComZero(data.getMinutes());
        const segundos = completarComZero(data.getSeconds());

        // Detecta partes do formato
        const partes = formato.split(' ');
        const formatoData = partes.find(p => p.toLowerCase().includes('dd')) || null;
        const formatoHora = partes.find(p => p.toLowerCase().includes('hh')) || null;

        // Monta a string
        let resultado = '';

        // Formata a data se necessário
        if (formatoData)
            resultado = `${dia}/${mes}/${ano}`;

        // Formata a hora se necessário
        if (formatoHora)
        {
            const separador = resultado ? ' ' : ''; // Espaço se já tiver data
            let horaFormatada = `${horas}:${minutos}`;

            // Adiciona segundos se necessário
            if (formatoHora.toLowerCase().includes('ss'))
                horaFormatada += `:${segundos}`;

            resultado += separador + horaFormatada;
        }

        return resultado;
    }


    private HandleInput(pEvent: InputEvent)
    {
        this.ValidateDate();
        const input = pEvent.target as HTMLInputElement;
        const value = input.value.replace(/\D/g, '');
        const placeholder = this.Input.placeholder;

        const [datePart, timePart] = placeholder.includes(' ') ?
            placeholder.split(' ') : [placeholder.startsWith('dd')
                ? placeholder : null, placeholder.startsWith('hh') ? placeholder : null];

        let formatted = '';
        let remainingDigits = value;

        if (datePart?.startsWith('dd/mm/aaaa'))
        {
            const dateDigits = remainingDigits.slice(0, 8);
            formatted = this.formatDateSection(dateDigits);
            remainingDigits = remainingDigits.slice(8);
        }

        if (timePart?.startsWith('hh'))
        {
            if (formatted !== '' && X.Length(formatted) == X.Length(datePart))
                formatted += ' ';
            formatted += this.formatTimeSection(remainingDigits, timePart === 'hh:MM:ss' ? 6 : 4);
        }
        if (pEvent.inputType == "deleteContentBackward")
            formatted = formatted.trim();
        input.value = formatted;
    }

    private formatDateSection(pDigits: string): string
    {
        let formatted = '';
        for (let i = 0; i < pDigits.length; i++)
        {
            if (i === 2 || i === 4)
                formatted += '/';
            formatted += pDigits[i];
        }
        return formatted;
    }

    private formatTimeSection(pDigits: string, pMax: number): string
    {
        let formatted = '';
        for (let i = 0; i < Math.min(pDigits.length, pMax); i++)
        {
            if (i === 2 || i === 4)
                formatted += ':';
            formatted += pDigits[i];
        }
        return formatted;
    }

    private ValidateDate()
    {
        this.Input.classList.remove('Error');
        if (X.IsEmpty(this.Input.value))
            return;
        if (!Date.IsDateOrTime(this.Input.value))
            this.Input.classList.add('Error');
        else
        {
            const [d, m, y] = this.Input.value.split('/');
            const date = new Date(`${y}-${m}-${d}`);
            if (this.Calendar.IsVisible)
                this.Calendar.SelectDate(date);
        }
    }

    override OnClick(pArg: KeyboardEvent)
    {
        this.Calendar.BindTo(this);
        this.Calendar.Show();
        this.Calendar.SelectedDate = this.SelectedDate;
    }
}