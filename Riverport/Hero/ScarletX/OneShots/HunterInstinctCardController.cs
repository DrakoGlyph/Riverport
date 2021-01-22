using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class HunterInstinctCardController : CardController
    {
        public HunterInstinctCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => c.IsOneShot));
        }

        public override IEnumerator Play()
        {
            var reveal = RevealCards_MoveMatching_ReturnNonMatchingCards(TurnTakerController, TurnTaker.Deck, false, false, true, new LinqCardCriteria(c => c.IsOneShot), 1, null, true, false, RevealedCardDisplay.ShowMatchingCards, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            var play = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => c.IsOneShot));
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
