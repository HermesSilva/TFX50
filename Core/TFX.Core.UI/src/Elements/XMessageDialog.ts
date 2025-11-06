/// <reference path="XDiv.ts" />

class XMessageDialog extends XBaseDialog 
{

    constructor(pOwner: XElement)
    {
        super(pOwner);
    }

    ShowError(pTitle: string, pMessage: string | Event, pDetail: string)
    {
        this.HTML.setAttribute("Type", "Error");
        this.Title = pTitle;
        if (this.IsVisible)
            this.Text += "\r\n\r\n****************************** \r\n\r\n" + pMessage + "\r\n" + pDetail;
        else
            this.Text = pMessage + "\r\n" + pDetail;
        this.Show();
    }
}
