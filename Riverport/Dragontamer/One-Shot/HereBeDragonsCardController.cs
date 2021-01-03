using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class HereBeDragonsCardController : CardController
    {
        public HereBeDragonsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")));
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")));

        }

        public override IEnumerator Play()
        {
            List<Card> played = new List<Card>();
            //Reveal cards from deck until a Dragon is revealed, put it into play
            var reveal = RevealCards_MoveMatching_ReturnNonMatchingCards(TurnTakerController, TurnTaker.Deck, true, false, false, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), 1, null, true, false, RevealedCardDisplay.ShowMatchingCards, true, false, played);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            //If no cards are played this way...
            if(played.Count == 0)
            {
                //...search your trash for a Dragon and put it into play
                var search = SearchForCards(HeroTurnTakerController, false, true, 1, 1, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), true, false, false, false);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            }
            //You may play a command card
            var command = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("command")), false);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(command); } else { this.GameController.ExhaustCoroutine(command); }
        }
    }
}
