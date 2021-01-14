using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace Riverport.Weaver
{
    public class RaidTheStockpilesCardController : CardController
    {
        public RaidTheStockpilesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(HeroTurnTaker.Deck, SuitMaterialFilter) ;
        }

        protected readonly LinqCardCriteria SuitMaterialFilter = new LinqCardCriteria(c => c.DoKeywordsContain("suit") || c.DoKeywordsContain("material"), "Suits or Materials", false, false, "Suit or Material", "Suits or Materials");

        public override IEnumerator Play()
        {
            var raid = RevealCards_MoveMatching_ReturnNonMatchingCards(TurnTakerController, TurnTaker.Deck, false, false, true, SuitMaterialFilter, 2, null, true, false, RevealedCardDisplay.ShowMatchingCards, true, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(raid); } else { this.GameController.ExhaustCoroutine(raid); }
            var play = SelectAndPlayCardFromHand(HeroTurnTakerController, associateCardSource: true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
