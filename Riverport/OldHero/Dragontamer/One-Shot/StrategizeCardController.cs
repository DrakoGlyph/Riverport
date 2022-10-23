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
            IEnumerator move;
            if (rca.FoundMatchingCards)
            {
                foreach (Card c in rca.MatchingCards)
                {
                    move = this.GameController.MoveCard(TurnTakerController, c, HeroTurnTaker.Hand, false, false, false, null, true, cardSource: GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
                }
            } else
            {
                move = this.GameController.SendMessageAction("There were no commands strategized", Priority.Medium, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
            
            //Console.WriteLine();
            var deploy = this.GameController.SelectAndMoveCard(HeroTurnTakerController, c=>c.Location == TurnTaker.Revealed && c.DoKeywordsContain("dragon"), HeroTurnTaker.Hand, false, cardSource:GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(deploy); } else { this.GameController.ExhaustCoroutine(deploy); }
            var clean = CleanupRevealedCards(TurnTaker.Revealed, TurnTaker.Deck, false, true, true, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(clean); } else { this.GameController.ExhaustCoroutine(clean); }
        }
    }
}
