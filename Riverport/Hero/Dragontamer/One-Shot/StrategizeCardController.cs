using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class StrategizeCardController : CardController
    {
        public StrategizeCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => c.DoKeywordsContain("command")));
        }

        public override IEnumerator Play()
        {
            List<RevealCardsAction> reveal = new List<RevealCardsAction>();
            var plan = this.GameController.RevealCards(TurnTakerController, TurnTaker.Deck, c => c.DoKeywordsContain("command"), 2, reveal, RevealedCardDisplay.ShowMatchingCards, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(plan); } else { this.GameController.ExhaustCoroutine(plan); }
            RevealCardsAction rca = reveal.FirstOrDefault();
            if (rca.FoundMatchingCards)
            {
                var move = BulkMoveCard(TurnTakerController, rca.MatchingCards, HeroTurnTaker.Hand, false, false, TurnTakerController, false);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
            List<Card> observe = new List<Card>();
            foreach(Card card in reveal.FirstOrDefault().NonMatchingCards)
            {
                Console.Write(card.Title + " ");
                if(card.DoKeywordsContain("dragon"))
                {
                    observe.Add(card);
                }
            }
            Console.WriteLine();
            IEnumerator deploy = null;
            if (observe.Count > 0)
                deploy = this.GameController.SelectAndPlayCard(HeroTurnTakerController, observe, false, true, null, GetCardSource());
            else
                deploy = this.GameController.SendMessageAction("No Dragons came to strategize", Priority.Medium, GetCardSource(), observe);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(deploy); } else { this.GameController.ExhaustCoroutine(deploy); }
            var clean = CleanupRevealedCards(TurnTaker.Revealed, TurnTaker.Deck, false, true, true, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(clean); } else { this.GameController.ExhaustCoroutine(clean); }
        }
    }
}
