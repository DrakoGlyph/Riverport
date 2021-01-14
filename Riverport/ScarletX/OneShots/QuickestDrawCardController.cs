using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class QuickestDrawCardController : CardController
    {
        public QuickestDrawCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<Card> results = new List<Card>();
            var draw = this.GameController.RevealCards(TurnTakerController, TurnTaker.Deck, 3, results, false, RevealedCardDisplay.ShowRevealedCards, null, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
            foreach(Card reveal in results)
            {
                Location destination = HeroTurnTaker.Hand;
                if (reveal.DoKeywordsContain("arrow")) destination = TurnTaker.PlayArea;
                var place = this.GameController.MoveCard(TurnTakerController, reveal, destination, false, false, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(place); } else { this.GameController.ExhaustCoroutine(place); }
            }
        }
    }
}
