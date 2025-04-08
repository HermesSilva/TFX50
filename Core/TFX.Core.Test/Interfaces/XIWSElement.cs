using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFX.Core.Test.Interfaces
{
    public interface XIWSBaseElement
    {
        Boolean IsValid
        {
            get;
        }
    }

    public interface XIWSElementValuable : XIWSBaseElement
    {

        string Value
        {
            get;
        }
        string GetAttribute(string pAttribute);
    }

    public interface XIWSTableColumn : XIWSElementValuable
    {
        public XIWSTableColumn Hader
        {
            get;
        }

        XIWSElementValuable GetElement(params XAtt[] pAttributes);
    }


    public interface XIWSTableRow
    {
        List<XIWSTableColumn> Column
        {
            get;
        }

    }

    public interface XIWSTableElement : XIWSBaseElement
    {
        void Parse(String pBody = "tbody", String pHeader = "th", String pHeaderData = "th");

        List<XIWSTableRow> Rows
        {
            get;
        }

        List<XIWSTableColumn> Header
        {
            get;
        }
    }

    public interface XIWSElement : XIWSBaseElement
    {

        Task ClickAsync();
        string GetInnerTexts();

        Task FillAsync(String pValue);

        string GetValue();
        Task EvaluateAsync(string pScrit);
        Task ClickToVisible();
        Task Focus();
    }

    public interface ISCElementHandle : XIWSBaseElement
    {
        List<ISCElementHandle> GetHandlers(String pTag);
    }
}
