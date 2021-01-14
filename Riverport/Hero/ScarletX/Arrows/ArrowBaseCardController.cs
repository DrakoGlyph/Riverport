using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections;

namespace Riverport.ScarletX
{
    public abstract class ArrowBaseCardController : CardController
    {
        protected ArrowBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        
        protected abstract IEnumerator FireArrow(GameAction gameAction = null);

        public override void AddTriggers()
        {
            AddBeforeDestroyAction(FireArrow);
        }
    }
}
