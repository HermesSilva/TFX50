/// <reference path="../Elements/Base/XBaseInput.ts" />
class XIntegerEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XIntegerEditor";
        this.AllowNegative = this.Mask.startsWith('-');
        this.MaxDigits = this.Mask.replace(/[^#0]/g, '').length;
        this.HasSeparator = this.Mask.includes('.');
        this.IsFixedMask = !this.Mask.includes('#') && /^[-]?0+$/.test(this.Mask);

        this.Init();
        this.Title = "Digite um Valor Inteiro";
    }

    Mask: string = "#.##0";
    AllowNegative: boolean = false;
    MaxDigits: number;
    HasSeparator: boolean;
    IsFixedMask: boolean;

    Init()
    {
        this.Input.value = this.FormatNumber('0');
        this.SetupEventListeners();
    }

    SetupEventListeners()
    {
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.GandleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.handleKeyDown);
        XEventManager.AddEvent(this, this.Input, XEventType.Blur, this.HandleBlur);
    }

    GandleInput(pArg: any)
    {
        const rawValue = this.GetRawValue(this.Input.value);
        const processed = this.ProcessValue(rawValue);
        this.Input.value = this.FormatNumber(processed);
    }

    handleKeyDown(e: any)
    {
        if ([8, 46, 9, 27, 13, 37, 38, 39, 40, 36, 35].Contains(e.keyCode))
            return;
        if ((e.ctrlKey || e.metaKey) && [67, 86, 88, 65].Contains(e.keyCode))
            return;

        const isNegativeSign = e.key === '-' && this.AllowNegative;
        const isNumber = e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode >= 96 && e.keyCode <= 105;

        if (isNegativeSign)
        {
            e.preventDefault();
            this.ToggleSign();
            return;
        }

        if (!isNumber)
        {
            e.preventDefault();
        }
    }

    ToggleSign()
    {
        const current = this.GetRawValue(this.Input.value);
        const newValue = current.startsWith('-') ? current.slice(1) : `-${current}`;
        this.Input.value = this.FormatNumber(newValue);
    }

    HandleBlur()
    {
        const rawValue = this.GetRawValue(this.Input.value);
        if (rawValue === '-' || rawValue === '')
        {
            this.Input.value = '0';
        } else
        {
            this.Input.value = this.FormatNumber(rawValue);
        }
    }

    GetRawValue(pValue:string)
    {
        return pValue.replace(/[^0-9-]/g, '');
    }

    ProcessValue(pValue: string)
    {
        let isNegative = this.AllowNegative && pValue.startsWith('-');
        let digits = pValue.replace(/-/g, '').replace(/^0+/, '') || '0';
        digits = digits.slice(0, this.MaxDigits);

        if (this.IsFixedMask)
        {
            digits = digits.RPad(this.MaxDigits, '0');
        }

        return (isNegative ? '-' : '') + digits;
    }

    FormatNumber(pValue: string)
    {
        const isNegative = pValue.startsWith('-');
        let digits = isNegative ? pValue.slice(1) : pValue;
        digits = digits.replace(/[^0-9]/g, '');

        if (this.HasSeparator)
        {
            digits = this.InsertSeparators(digits);
        }

        if (this.IsFixedMask)
        {
            digits = digits.LPad(this.MaxDigits, '0');
        }

        return (isNegative ? '-' : '') + digits;
    }

    InsertSeparators(pDigits: string): any
    {
        return pDigits?.split('')?.reverse()?.join('')?.match(/.{1,3}/g)?.join('.').split('').reverse().join('').replace(/^\./, '');
    }
}