
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.AspNetCore.Http;

using TFX.Core.Exceptions;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;
using TFX.Core.Model;

namespace TFX.Core.Data
{
    public static class XResource
    {
        private static Dictionary<Assembly, String[]> _ResourceNames = new Dictionary<Assembly, String[]>();
        private static Dictionary<String, Byte[]> _Streams = new Dictionary<String, Byte[]>();
        private static readonly Object _ToLock = new Object(); 
        public static Stream GetResourceStream(Assembly pAssembly, String pFile)
        {
            lock (_ToLock)
            {
                String name;
                pFile = pFile.SafeLower();

                if (_ResourceNames.ContainsKey(pAssembly))
                {
                    String[] source = _ResourceNames[pAssembly];
                    name = source.FirstOrDefault(d => d.SafeLower().EndsWith(pFile));
                }
                String[] names = pAssembly.GetManifestResourceNames();
                if (names.IsEmpty())
                    return null;
                _ResourceNames[pAssembly] = names;
                name = names.FirstOrDefault(d => d.SafeLower().EndsWith(pFile));

                if (name.IsEmpty())
                    return null;
                return pAssembly.GetManifestResourceStream(name);
            }
        }

        public static Stream GetResourceStream<T>(String pFile)
        {
            return GetResourceStream(typeof(T).Assembly, pFile);    
        }
    }

}
