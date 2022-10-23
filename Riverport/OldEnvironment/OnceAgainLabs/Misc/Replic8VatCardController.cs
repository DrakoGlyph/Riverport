using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class Replic8VatCardController : CardController
    {
        public Replic8VatCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.DoKeywordsContain("scientist"), Revive, TriggerType.RevealCard, TriggerTiming.After);
        }

        private IEnumerator Revive(DestroyCardAction arg)
        {
            var reveal = RevealCards_PutSomeIntoPlay_DiscardRemaining(TurnTakerController, TurnTaker.Deck, null, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), revealUntilNumberOfMatchingCards: 1);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            var destroy = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }
    }
}
