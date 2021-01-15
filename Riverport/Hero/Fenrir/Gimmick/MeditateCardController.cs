using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class MeditateCardController : FenrirBaseCardController
    {
        public MeditateCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AllowFastCoroutinesDuringPretend = false;
        }

        public override void AddTriggers()
        {
            AddTrigger<DrawCardAction>((DrawCardAction dca) => dca.HeroTurnTaker == HeroTurnTaker, Meditate, TriggerType.CancelAction, TriggerTiming.Before);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => IsFenrir(dda.Target) || IsFenrir(dda.DamageSource.Card), DestroyThisCardResponse, TriggerType.DestroySelf, TriggerTiming.After);
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Unfocus, TriggerType.DestroySelf);
        }

        private IEnumerator Unfocus(PhaseChangeAction arg)
        {
            if(ShouldActivate("wolf"))
            {
                var draw = DrawCard();
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                var poof = this.GameController.DestroyCard(HeroTurnTakerController, Card, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(poof); } else { this.GameController.ExhaustCoroutine(poof); }
            }
        }

        private IEnumerator Meditate(DrawCardAction arg)
        {
            var search = SearchForCards(HeroTurnTakerController, true, false, 1, 1, new LinqCardCriteria(c => true), false, true, false, shuffleAfterwards: true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            var cancel = CancelAction(arg, false);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(cancel); } else { this.GameController.ExhaustCoroutine(cancel); }

        }
    }
}
