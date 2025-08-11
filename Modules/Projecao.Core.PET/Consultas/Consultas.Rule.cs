using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.PET.Agendamento;
using Projecao.Core.PET.Consultas;

using TFX.Core.Data;
using TFX.Core.Exceptions;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Model.Data;
using Projecao.Core.ERP.DB;
using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;
using System.IO;
using TFX.Core;
using TFX.iText7;
using TFX.Core.Model.DIC.RPT.Model;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.RPT;
using Projecao.Core.PET.RPTs.Receita;

namespace Projecao.Core.PET.Anamnesia
{
    [XRegister(typeof(AnamnesiaRule), sCID, typeof(ConsultasSVC))]
    public class AnamnesiaRule : ConsultasSVC.XRule
    {
        public const String sCID = "479BF3E7-A9D9-46A3-877D-F0EEA47A5023";
        public static Guid gCID = new Guid(sCID);

        public AnamnesiaRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, ConsultasSVC pModel, ConsultasSVC.XDataSet pDataSet)
        {
            ERPx._ERPxProfissionalCategoria profcat = GetTable<ERPx._ERPxProfissionalCategoria>();
            if (!profcat.Open(ERPx.ERPxProfissionalCategoria.ERPxProfissionalID, pContext.Logon.UserID, ERPx.ERPxProfissionalCategoria.ERPxCategoriaID, ERPxCategoria.XDefault.Saude_Animal))
                throw new XUnconformity($"Usuário corrente não é um profissional de {ERPxCategoria.XDefault.sEstetica_Animal}, não é permitido alterar consulta.");
        }

        protected override void GetWhere(XExecContext pContext, ConsultasSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ConsultasSVC.xPETxAtendimento.PETxAtendimentoClasseID, XOperator.In, new[] { PETxAtendimentoClasse.XDefault.Saude, PETxAtendimentoClasse.XDefault.Todas });
        }

        protected override void BeforeChangeState(XExecContext pContext, ConsultasSVC pModel, ConsultasSVC.XDataSet pDataSet)
        {
            if (pDataSet.ConsultasDocumentosDataSet.Tuples.Any(t => t.SYSxEstadoID == SYSxEstado.XDefault.Inativo && pDataSet.Current?.PETxAnamnesiaID != t.PETxAnamnesiaID))
                throw new XUnconformity("Não é permitido deletar documentos antigos.");
        }

        protected override void AfterGet(XExecContext pContext, ConsultasSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            ERPx._ERPxProfissionalCategoria profcat = GetTable<ERPx._ERPxProfissionalCategoria>();
            Boolean usernovet = !profcat.Open(ERPx.ERPxProfissionalCategoria.ERPxProfissionalID, pContext.Logon.UserID, ERPx.ERPxProfissionalCategoria.ERPxCategoriaID, ERPxCategoria.XDefault.Saude_Animal);

            AgendamentoProfiSaudeSVC.XService prof = GetService<AgendamentoProfiSaudeSVC.XService>();
            Guid[] atends = pDataSet.Tuples.Select(t => t.PETxAnamnesiaID).Distinct().ToArray();
            prof.MaxRows = 0;
            prof.Open(AgendamentoProfiSaudeSVC.xPETxAtendimentoProfissional.PETxAtendimentoID, XOperator.In, atends);
            foreach (ConsultasSVC.XTuple tpl in pDataSet)
            {
                if (usernovet)
                    pDataSet.SetReadOnly();
                tpl.Profissionais = String.Join(", ", prof.DataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAnamnesiaID).Select(t => t.Nome));
                Boolean isreadonly = tpl.SYSxEstadoID != SYSxEstado.XDefault.Ativo || tpl.PETxAtendimentoEstadoID != PETxAtendimentoEstado.XDefault.Criado;
                tpl.IsReadOnly = isreadonly;
                pDataSet.ConsultasDocumentosDataSet.Tuples.Where(t => t.PETxAnimalID != tpl.PETxAnimalID).ForEach(t => t.IsReadOnly = isreadonly);
                pDataSet.ConsultasDescritivoDataSet.Tuples.Where(t => t.PETxAnimalID != tpl.PETxAnimalID).ForEach(t => t.IsReadOnly = isreadonly);
            }
        }
    }
}