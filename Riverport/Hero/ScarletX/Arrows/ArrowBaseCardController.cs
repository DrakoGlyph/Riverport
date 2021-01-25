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
            //AllowFastCoroutinesDuringPretend = false;
        }

        protected virtual TriggerType Trigger
        {
            get;
        }

        protected abstract IEnumerator FireArrow(DestroyCardAction dca = null);

       

        public override void AddTriggers()
        {
            AddWhenDestroyedTrigger(FireArrow, Trigger);
        }

    }
}
