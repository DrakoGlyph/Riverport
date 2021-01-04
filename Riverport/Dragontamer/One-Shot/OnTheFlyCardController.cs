using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class OnTheFlyCardController : CardController
    {
        public OnTheFlyCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => c.DoKeywordsContain("command")));
        }

        public override IEnumerator Play()
        {
            List<SelectWordDecision> storedResults = new List<SelectWordDecision>();
            //Choose Dragon or Command
            var choose = this.GameController.SelectWord(HeroTurnTakerController, new List<string>() { "dragon", "command" }, SelectionType.SearchDeck, storedResults, false, null, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(choose); } else { this.GameController.ExhaustCoroutine(choose); }
            //Reveal cards from your deck until you reveal a command. Put it into your hand
            var reveal = RevealCards_MoveMatching_ReturnNonMatchingCards(TurnTakerController, TurnTaker.Deck, false, false, true, new LinqCardCriteria(c => c.DoKeywordsContain(GetSelectedWord(storedResults))), 1, null, true, false, RevealedCardDisplay.ShowMatchingCards, true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            //You may play a command
            var play = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("command") || c.DoKeywordsContain("dragon")));
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
