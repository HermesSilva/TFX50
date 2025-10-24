using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TFX.Core.Cache;
using TFX.Core.Controllers;
using TFX.Core.Data.Servicos.Menu;
using TFX.Core.Services;

namespace TFX.Core.Data.Servicos.Menu.Rules
{
    public class MenuRule : BaseMenuRule
    {
        public MenuRule(XService pService)
               :base(pService)
        {
        }

        protected override List<MenuTuple> AfterSelect(List<MenuTuple> pTuples)
        {
            var tpl= pTuples.FirstOrDefault();
            pTuples.Clear();
            foreach (var item in XMainCache.Apps)
            {
                var tuple = new MenuTuple { 
                    Menu = { Value = tpl.Menu.Value },
                    Item = { Value = item.Value.Title },
                    CORxMenuItemID = { Value = item.Key },
                    CORxRecursoID = { Value = item.Key },
                    CORxMenuID = { Value = tpl.CORxMenuID.Value}
                };
                pTuples.Add(tuple);
            }
            return base.AfterSelect(pTuples);
        }
    }
}