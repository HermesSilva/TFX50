using System;
using System.IO;
using System.Net;
using System.Linq;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.LZMA;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC;
using TFX.Core.Model.Interfaces;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Web;

namespace Projecao.Core.ITG.RI
{
    public class XReverseIntegration
    {
        internal static XAfterCommitEvent ReverseIntegration(HttpContext pHttpContext, XExecContext pContext)
        {
            if (!Directory.Exists(XDefault.DataTemp))
                Directory.CreateDirectory(XDefault.DataTemp);
            String path = Path.Combine(XDefault.DataTemp, DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss"));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (XMemoryStream ms = new XMemoryStream())
            {
                pHttpContext.Request.Body.CopyTo(ms);
                ms.Position = 0;
                while (ms.Position < ms.Length)
                {
                    ms.Read(out String filename);
                    ms.Read(out Byte[] data);
                    File.WriteAllBytes(Path.Combine(path, filename+".bin"), XLzma.Decode(data));
                    XConsole.Warn($"DataReceived FileName[{filename}] DataSize[{data.SafeLength()}]");
                }
            }
            return null;
        }          
    }
}