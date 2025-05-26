/// <reference path="Base/XElement.ts" />
/// <reference path="../Reflection/XReflections.ts" />

class XTagEditor extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Title = XUtils.AddElement<HTMLSpanElement>(this.HTML, "span", "XTagEditorTitle");
        this.Input = XUtils.AddElement<HTMLInputElement>(this.HTML, "input", "XTagEditorInput");
        this.Title.addEventListener('click', () => this.Input.focus(), false);
        this.Title.innerHTML = "Tag Editor";
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "XTagEditorSVG");
        this.SVG.src = "svg/closered.svg";
        this.Input.addEventListener('input', () => this.onInput(this.Input, ""), false)
    }
    Input: HTMLInputElement;
    Title: HTMLSpanElement;
    SVG: HTMLImageElement;

    private onInput(pInput: HTMLInputElement, pMask?: string)
    {
        if (pMask)
        {
            const digits = pInput.value.replace(/\D/g, '')
            pInput.value = this.ApplyMask(digits, pMask)
        }
        XUtils.ApplySize(pInput, pInput.value, pInput.style.font);
    }

    private ApplyMask(val: string, mask: string): string
    {
        let out = ''
        let vi = 0
        let seenHashes = 0

        for (const m of mask)
        {
            if (m === '#')
            {
                seenHashes++
                if (vi < val.length)
                    out += val[vi++]
            }
            else if (val.length > seenHashes)
                out += m
        }

        return out
    }
}

class XEditableTag extends XDiv 
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XEditableTag");
        this.Editor = new XTagEditor(this, "XTagEditor");
        this.Editor.SVG.addEventListener("click", () => this.DoClick(), false);
    }
    Editor: XTagEditor;
    OnClick!: (pTag: XEditableTag) => void;
    DoClick()
    {
        if (this.OnClick != null)
            this.OnClick.apply(this, [this]);
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }
}