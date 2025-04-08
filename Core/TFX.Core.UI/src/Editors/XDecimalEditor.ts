/// <reference path="../Elements/Base/XBaseInput.ts" />
class XDecimalEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XDecimalEditor";
        this.AllowNegative = false;
        this.MaxIntegerDigits = 4;
        this.DecimalDigits = 2;

        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.HandleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.HandleKeydown);
        this.Input.value = this.FormatValue(this.ProcessValue(''));
        this.Title = "Digite um Valor Decimal";
    }

    AllowNegative: boolean;
    MaxIntegerDigits: number;
    DecimalDigits: number;

    private HandleInput(event: Event)
    {
        const position = this.Input.selectionStart;
        const value = this.Input.value;

        const processed = this.ProcessValue(value);
        const formatted = this.FormatValue(processed);

        if (this.Input.value !== formatted)
        {
            this.Input.value = formatted;
            this.AdjustCursorPosition(position, value, formatted);
        }
    }

    private HandleKeydown(pArg: KeyboardEvent)
    {
        if (pArg.key === '-')
        {
            pArg.preventDefault();
            if (this.AllowNegative)
            {
                const processed = this.ProcessValue(this.Input.value);
                processed.isNegative = !processed.isNegative;
                this.Input.value = this.FormatValue(processed);
            }
        }
        else
            if (pArg.ctrlKey && (pArg.key === 'Home' || pArg.key === 'End'))
            {
                pArg.preventDefault();
                this.Input.setSelectionRange(0, this.Input.value.length);
            }
            else
                if (pArg.key === ',' && this.Input.value.includes(','))
                    pArg.preventDefault();
    }

    private ProcessValue(value: string): { isNegative: boolean; integerPart: string; decimalPart: string }
    {
        let rawValue = value.replace(/[^\d-,]/g, '');
        let isNegative = false;

        if (this.AllowNegative)
        {
            isNegative = rawValue.startsWith('-');
            rawValue = rawValue.replace(/-/g, '');
        }

        const [integer = '0', decimal = ''] = rawValue.split(',');

        const integerClean = integer.replace(/\D/g, '').replace(/^0+/, '').substring(0, this.MaxIntegerDigits) || '0';

        const decimalClean = decimal.replace(/\D/g, '').slice(-this.DecimalDigits).RPad(this.DecimalDigits, '0');

        return {
            isNegative: isNegative && this.AllowNegative,
            integerPart: integerClean || '0',
            decimalPart: decimalClean
        };
    }

    private FormatValue(processed: { isNegative: boolean; integerPart: string; decimalPart: string }): string
    {
        const formattedInteger = processed.integerPart
            .split('')
            .reverse()
            .join('')
            .replace(/(\d{3})(?=\d)/g, '$1.')
            .split('')
            .reverse()
            .join('')
            .replace(/^\./, '') || '0';

        const sign = processed.isNegative ? '-' : '';
        return `${sign}${formattedInteger},${processed.decimalPart}`;
    }

    private AdjustCursorPosition(oldPos: number | null, oldValue: string, newValue: string)
    {
        if (oldPos === null)
            return;

        const commaIndex = newValue.indexOf(',');
        const isDecimal = oldPos > oldValue.indexOf(',');

        if (isDecimal && commaIndex !== -1)
        {
            const decimalCursor = Math.min(oldPos - oldValue.indexOf(',') - 1 + commaIndex + 1, newValue.length);
            this.Input.setSelectionRange(decimalCursor, decimalCursor);
        }
        else
            this.Input.setSelectionRange(oldPos, oldPos);
    }
}