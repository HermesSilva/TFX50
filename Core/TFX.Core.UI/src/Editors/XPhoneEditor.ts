/// <reference path="../Elements/Base/XBaseInput.ts" />


class PhoneFormatter
{
    static format(value: string): string
    {
        let nums = value.replace(/\D/g, '');
        let formatted = '';

        // Limitar tamanho máximo
        const maxLength = nums.startsWith('55') ? 13 : 11;
        nums = nums.substring(0, maxLength);

        if (nums.startsWith('55') && nums.length > 2)
        {
            formatted = `+55 `;
            nums = nums.substring(2);
        }

        if (nums.length >= 2)
        {
            formatted += `(${nums.substring(0, 2)})`;
            nums = nums.substring(2);
        }

        if (nums.length > 0)
        {
            formatted += ' ' + nums.replace(/(\d{4,5})(\d{4})$/, '$1-$2');
        }

        return formatted.replace(/(\s)-/g, '$1').trim();
    }

    static validate(phone: string): boolean
    {
        return /^(?:\+55\s\(\d{2}\)\s\d{5}-\d{4}$|\(\d{2}\)\s\d{4,5}-\d{4}$)/.test(phone);
    }
}

class XPhoneEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XPhoneEditor";
        this.Title = "Digite um Nº de Telefone";
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.handleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.handleKeyDown);
    }
    private lastValue = '';
    private cursorPos = 0;


    private handleKeyDown(e: KeyboardEvent): void
    {
        // Permitir navegação e comandos especiais
        const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Home', 'End', 'Tab', 'Control'];

        if (allowedKeys.Contains(e.key) || e.ctrlKey)
            return;

        if (!/\d/.test(e.key))
            e.preventDefault();
    }

    private handleInput(): void
    {
        const prevValue = this.Input.value;
        const rawValue = prevValue.replace(/\D/g, '');

        // Controlar máximo de dígitos
        const maxLength = rawValue.startsWith('55') ? 13 : 11;
        const newValue = PhoneFormatter.format(rawValue.substring(0, maxLength));

        // Manter posição do cursor
        const cursorBefore = this.Input.selectionStart || 0;
        const diff = newValue.length - prevValue.length;

        // Ajustar posição do cursor
        let newCursorPos = cursorBefore;
        if (this.lastValue.length > newValue.length)
        {
            newCursorPos = this.calculateCursorPos(cursorBefore, prevValue, newValue);
        } else
        {
            newCursorPos = cursorBefore + diff;
        }

        this.Input.value = newValue;
        this.lastValue = newValue;

        // Corrigir posição do cursor
        requestAnimationFrame(() =>
        {
            this.Input.setSelectionRange(newCursorPos, newCursorPos);
        });

        this.updateValidation();
    }

    private calculateCursorPos(oldPos: number, oldValue: string, newValue: string): number
    {
        let adjust = 0;
        const isBackspace = oldValue.length > newValue.length;

        // Ajustar para formatação automática
        for (let i = 0; i < oldPos; i++)
        {
            if (isBackspace && /[()\-\s]/.test(oldValue[i]))
            {
                adjust--;
            }
            if (!isBackspace && /[()\-\s]/.test(newValue[i]))
            {
                adjust++;
            }
        }

        return Math.max(0, Math.min(newValue.length, oldPos + adjust));
    }

    private updateValidation(): void
    {
        const isvalid = PhoneFormatter.validate(this.Input.value);

        this.Input.classList.remove('Error');
        if (isvalid)
            return;
        if (!isvalid)
            this.Input.classList.add('Error');

    }

}