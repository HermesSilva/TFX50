using System.Globalization;
using System.Text;

namespace TFX.Core
{
    public static class XEncoding
    {
        public readonly static CultureInfo DefaultCulture = new CultureInfo("pt-BR");
        public readonly static CultureInfo En_US = new CultureInfo("en-US");
        public readonly static Encoding Default = new UTF8Encoding();
    }
}