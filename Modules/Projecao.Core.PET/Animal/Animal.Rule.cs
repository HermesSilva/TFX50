using System;
using System.Linq;

using TFX.Core;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.Animal
{
    [XRegister(typeof(AnimalRule), sCID, typeof(AnimalSVC))]
    public class AnimalRule : AnimalSVC.XRule
    {
        public const String sCID = "B96D3567-58D9-4FA3-BC4B-A72ED434C334";
        public static Guid gCID = new Guid(sCID);

        public AnimalRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, AnimalSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t => t.Idade = CalcIdade(t.Nascimento));
        }

        protected override void BeforeFlush(XExecContext pContext, AnimalSVC pModel, AnimalSVC.XDataSet pDataSet)
        {
            if (pDataSet.AnimalImagemDataSet.Count > 0 && !pDataSet.AnimalImagemDataSet.Tuples.SafeAny(t => t.Principal))
                pDataSet.AnimalImagemDataSet.Tuples.FirstOrDefault().Principal = true;
        }

        private String CalcIdade(DateTime pNascimento)
        {
            if (pNascimento < XDefault.MinValidDate)
                return "";
            TimeSpan ts = DateTime.Now - pNascimento;
            Double meses = ((DateTime.Now.Year - pNascimento.Year) * 12) + DateTime.Now.Month - pNascimento.Month;
            Double anos = (Int32)(meses / 12);
            Double mesres = meses - anos * 12;
            switch (ts.TotalDays)
            {
                case Double n when n < 60:
                    if (ts.TotalDays <= 5)
                        return $"recem nascido";
                    return $"{ts.TotalDays} dias";

                case Double n when n <= 366:
                    return $"{meses} meses";

                default:
                    String mes = mesres > 1 ? $"{mesres} meses" : $"{mesres} mês";
                    if (mesres == 0)
                        mes = "";
                    String ano = anos > 1 ? $"{anos} anos" : "{anos} ano";
                    if (anos == 0)
                        ano = "";
                    if (mesres > 0)
                        return $"{ano} e {mes}";
                    return $"{ano}";
            }
        }
    }
}