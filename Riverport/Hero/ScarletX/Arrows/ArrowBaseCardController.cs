using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;

namespace Riverport.ScarletX
{
    public abstract class ArrowBaseCardController : CardController
    {
        protected ArrowBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AllowFastCoroutinesDuringPretend = false;
        }

        
        protected abstract IEnumerator FireArrow(GameAction ga = null);

       

        public override void AddTriggers()
        {
            AddBeforeDestroyAction(FireArrow);
        }

    }
}
