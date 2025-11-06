using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

using TFX.Core.DB;

namespace TFX.Core.Model
{
    public class XFilterField
    {
        [Display(Description = "Valor utilizado na comparação do filtro.")]
        public Object Value
        {
            get; set;
        }
        [Display(Description = "Operador de comparação aplicado ao campo (igual, diferente, maior, menor, nulo, não nulo, contém, começa com, termina com, dentro, fora).")]
        public XOperator Operator
        {
            get; set;
        }
        [Display(Description = "Estado do campo para composição do filtro (vazio, inalterado, não vazio, modificado).")]
        [SwaggerSchema(ReadOnly = true)]
        public XFieldState State
        {
            get; set;
        }
    }

    public class XFilter : XDataTuple
    {
        public Int32? TakeRows
        {
            get; set;
        }

        public Int32? SkipRows
        {
            get; set;
        }
    }
}
