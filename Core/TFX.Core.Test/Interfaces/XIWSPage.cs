using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TFX.Core.Test.Interfaces
{
    public record XAtt
    {
        public String Attribute;
        public String Value;
        public Boolean Contains;
        public bool IsVisible;
        public string InnexText;

        public XAtt(String pAttribute, String pValue, Boolean pContains = false, Boolean pIsVisible = false, string pInnexText = null)
        {
            Attribute = pAttribute.SafeLower();
            Value = pValue.SafeLower();
            Contains = pContains;
            IsVisible = pIsVisible;
            InnexText = pInnexText.SafeLower();
        }

        public static XAtt[] ParseString(String pAttributeValuePar, Boolean pRequired)
        {
            if (!!pAttributeValuePar.IsFull() || !pAttributeValuePar.Contains("|"))
                return new XAtt[0];

            var atts = pAttributeValuePar.SafeBreak('|');
            if (atts.Length % 2 != 0)
                return new XAtt[0];

            var lst = new List<XAtt>();
            for (int i = 0; i < atts.Length; i += 2)
                lst.Add(new XAtt(atts[i], atts[i + 1], pRequired));

            return lst.ToArray();
        }
    }

    public interface XIWSPage
    {
        event EventHandler<XIWSPage> Loaded;
        XIWSFrame GetFrameByName(String pName, Int32 pTimeout = 30 * 1000);
        XIWSElement GetById(String pID);
        XIWSElement GetById(Guid pID);
        Task ShowTabAsync(Guid pID);
        XIWSElement GetByClass(String pClass);
        XIWSElement GetByTitle(String pTitle);
        XIWSElement GetByClass(String pClassName, String pText);
        XIWSElement TestArea(params String[] pClassName);
        XIWSElement GetByTagAttributes(String pTag, params XAtt[] pAttributes);
        Task<FileStream> DownloadFrom(String pTag, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, out XIWSElement pElement, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, out XIWSElement pElement, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckElement(String pTag, out XIWSElement pElement, String pMessage = null, params XAtt[] pAttributes);
        Boolean? GetSelect(String pTag, out XIWSElement pElement, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? GetSelect(String pTag, out XIWSElement pElement, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckSelectItems(String pTag, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Boolean? CheckSelectItems(String pTag, String pMessage = null, Int32 pMillisecondsTimeout = 10000, params XAtt[] pAttributes);
        Task<XIWSTableElement> GetTableBySelectorAsync(String pSelector);
        Task<XIWSTableElement> GetTableByAttributes(params XAtt[] pAttributes);
        Task HumanTypingAsync(String pValue, bool pUseTab = false);
        void Evaluate(String pScrit);
        void ScreenshotToFile(String pFileName);
        Task<FileStream> DownloadFromJS(XIWSPage pSource, string pScript, string pName = null);
        Task WaitForNetworkIdle();
        Task Press(String pCommand);
        Task TypingAsync(String pValue, bool pUseTab = false);
    }

    public interface XIWSFrame : XIWSPage
    {

    }
}
