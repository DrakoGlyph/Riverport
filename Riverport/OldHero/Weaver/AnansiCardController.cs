using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class AnansiCardController : CardController
    {
        public AnansiCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        
        protected readonly LinqCardCriteria PatchFilter = new LinqCardCriteria(c => c.DoKeywordsContain("patch"), "Patches", false, false, "Patch", "Patches");


        public override void AddTriggers()
        {
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Card, c => c == Card, TargetType.All, 2, DamageType.Psychic);
            AddTrigger<DestroyCardAction>(dca => dca.CardToDestroy.Card.DoKeywordsContain("patch"), Replace, TriggerType.MoveCard, TriggerTiming.After);
        }

        private IEnumerator Replace(DestroyCardAction arg)
        {
            arg.SetPostDestroyDestination(HeroTurnTaker.Hand, cardSource: GetCardSource());
            return DoNothing();
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var craft = SearchForCards(HeroTurnTakerController, true, false, 1, 1, PatchFilter, true, false, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(craft); } else { this.GameController.ExhaustCoroutine(craft); }
            var pay = DealDamage(Card, Card, 2, DamageType.Psychic);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
        }
    }
}
