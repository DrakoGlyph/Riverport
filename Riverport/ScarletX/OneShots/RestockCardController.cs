using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class RestockCardController : CardController
    {
        public RestockCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var restock = this.GameController.ShuffleTrashIntoDeck(TurnTakerController, false, null, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(restock); } else { this.GameController.ExhaustCoroutine(restock); }
            var pull = RevealCards_MoveMatching_ReturnNonMatchingCards(TurnTakerController, TurnTaker.Deck, false, true, false, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")), 2, null, true, false, RevealedCardDisplay.ShowMatchingCards);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pull); } else { this.GameController.ExhaustCoroutine(pull); }
        }
    }
}
