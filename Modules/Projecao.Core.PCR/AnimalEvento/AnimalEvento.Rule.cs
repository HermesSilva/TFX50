using System;

using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.AnimalEvento
{
    [XRegister(typeof(AnimalEventoRule), sCID, typeof(AnimalEventoSVC))]
    public class AnimalEventoRule : AnimalEventoSVC.XRule
    {
        public const String sCID = "630927B7-7944-4F2F-87CC-DC392600C809";
        public static Guid gCID = new Guid(sCID);

        public AnimalEventoRule()
        {
            ID = gCID;
        }

        private AnimalSVC.XService _Animal;

        protected override void AfterFlush(XExecContext pContext, AnimalEventoSVC pModel, AnimalEventoSVC.XDataSet pDataSet)
        {
            _Animal.Flush();
        }

        protected override void BeforeFlush(XExecContext pContext, AnimalEventoSVC pModel, AnimalEventoSVC.XDataSet pDataSet)
        {
            _Animal = GetService<AnimalSVC.XService>();
            _Animal.FilterZero = true;
            AnimalEventoSVC.XService evento = GetService<AnimalEventoSVC.XService>();
            foreach (AnimalEventoSVC.XTuple tpl in pDataSet)
            {
                if (tpl.PCRxAnimalID.IsEmpty())
                    throw new XError($"Não é permitido evento para o animal com ID=[{tpl.PCRxAnimalID}].");
                if (!PCRxEventoTipo.Instance.StaticData.SafeAny<PCRxEventoTipo.XTuple>(t => t.PCRxEventoTipoID == tpl.PCRxEventoTipoID && t.Animal))
                    throw new XError($"O evento com código [{tpl.PCRxEventoTipoID}] não é um evento para animais.");

                _Animal.OpenAppend(tpl.PCRxAnimalID);
                AnimalSVC.XTuple anltpl = _Animal.Current;
                anltpl.PCRxEventoReprodutivoID = tpl.PCRxEventoID;
                if (tpl.PCRxAnimalLoteID.IsFull())
                    anltpl.PCRxAnimalLoteID = tpl.PCRxAnimalLoteID;
                switch (tpl.PCRxEventoTipoID)
                {
                    case PCRxEventoTipo.XDefault.Aborto:
                    case PCRxEventoTipo.XDefault.Natimorto:
                        anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Solteira;
                        anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID);
                        break;

                    case PCRxEventoTipo.XDefault.Morte:
                        anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Morto;
                        anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID) + " (morte)";
                        if (anltpl.PCRxMaeID.IsFull() && _Animal.OpenAppend(anltpl.PCRxMaeID))
                        {
                            anltpl = _Animal.Current;
                            anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Solteira;
                            anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID);
                        }
                        break;

                    case PCRxEventoTipo.XDefault.Venda:
                        anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Vendida;
                        anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID) + " Vendida";
                        break;

                    case PCRxEventoTipo.XDefault.Parto:
                        anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Parida;
                        anltpl.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Vaca;
                        anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID) + " Parida";
                        anltpl.NumeroPartos++;
                        break;

                    case PCRxEventoTipo.XDefault.Desmama:
                        if (anltpl.PCRxMaeID.IsFull() && _Animal.OpenAppend(anltpl.PCRxMaeID))
                        {
                            _Animal.Current.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Solteira;
                            _Animal.Current.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Vaca;
                            _Animal.Current.Nome = PCRxAnimalFase.XDefault.GetTitle(_Animal.Current.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(_Animal.Current.PCRxRacaID) + " Solteira";
                        }
                        anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Engorda;
                        anltpl.PCRxAnimalFaseID = anltpl.ERPxGeneroID == ERPxGenero.XDefault.Masculino ? PCRxAnimalFase.XDefault.Garrote : PCRxAnimalFase.XDefault.Novilha;
                        anltpl.Nome = PCRxAnimalFase.XDefault.GetTitle(anltpl.PCRxAnimalFaseID) + " " + PCRxRaca.XDefault.GetTitle(anltpl.PCRxRacaID) + " Engorda";
                        break;

                    case PCRxEventoTipo.XDefault.Doenca:
                    case PCRxEventoTipo.XDefault.Outros:
                        break;

                    case PCRxEventoTipo.XDefault.IATF:
                        switch (tpl.PCRxIATFFaseTipoID)
                        {
                            case PCRxIATFFaseTipo.XDefault.IATF1:
                            case PCRxIATFFaseTipo.XDefault.IATF3:
                            case PCRxIATFFaseTipo.XDefault.IATF2:
                                anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Inceminado;
                                tpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Inceminado;
                                tpl.PCRxReprodutorID = tpl.PCRxReprodutorID;
                                break;

                            case PCRxIATFFaseTipo.XDefault.DG1:
                            case PCRxIATFFaseTipo.XDefault.DG2:
                            case PCRxIATFFaseTipo.XDefault.DG3:
                            case PCRxIATFFaseTipo.XDefault.DG_Final:
                                if (tpl.PCRxAnimalEstadoID == PCRxAnimalEstado.XDefault.Prenha)
                                {
                                    anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Prenha;
                                    tpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Prenha;
                                    evento.MaxRows = 1;
                                    if (anltpl.PCRxEventoReprodutivoID.IsFull() && evento.Open(AnimalEventoSVC.xPCRxAnimalEvento.PCRxAnimalEstadoID, PCRxAnimalEstado.XDefault.Inceminado, AnimalEventoSVC.xPCRxAnimalEvento.PCRxAnimalID, anltpl.PCRxAnimalID))
                                        tpl.PCRxReprodutorID = evento.Current.PCRxReprodutorID;
                                }
                                else
                                    if (tpl.PCRxIATFFaseTipoID == PCRxIATFFaseTipo.XDefault.DG_Final)
                                    anltpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.NA;
                                break;

                            case PCRxIATFFaseTipo.XDefault.Implante_Dispositivo:
                            case PCRxIATFFaseTipo.XDefault.Retirada_do_Implante:
                            default:
                                break;
                        }
                        break;

                    case PCRxEventoTipo.XDefault.Vacinacao:
                    case PCRxEventoTipo.XDefault.Nascimento:
                    case PCRxEventoTipo.XDefault.Pesagem:
                        break;

                    default:
                        throw new XError($"O evento [{PCRxEventoTipo.XDefault.GetTitle(tpl.PCRxEventoTipoID)}] com código [{tpl.PCRxEventoTipoID}] não foi tratado.");
                }
            }
        }
    }
}