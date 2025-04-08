using TFX.Core.Model;

namespace TFX.Access
{
    public abstract class XTest<Rule, Tuple>
        where Tuple : XServiceDataTuple
        where Rule : XTestRule<Tuple>, new()
    {
        protected Rule CreateRule()
        {
            return new Rule();
        }
    }
}
