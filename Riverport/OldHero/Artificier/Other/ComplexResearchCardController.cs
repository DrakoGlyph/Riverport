using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public class ComplexResearchCardController : ArtificierBaseCardController
    {
        public ComplexResearchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<Card> revealed = new List<Card>();
            var reveal = this.GameController.RevealCards(TurnTakerController, HeroTurnTaker.Deck, 5, revealed, false, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            foreach(Card c in revealed)
            {
                Location moveTo = TurnTaker.Trash;
                if (c.DoKeywordsContain("relic")) moveTo = TurnTaker.Deck;
                if (c.DoKeywordsContain("spell") || c.DoKeywordsContain("enchantment")) moveTo = HeroTurnTaker.Hand;
                var move = this.GameController.MoveCard(TurnTakerController, c, moveTo, false, false, true, null, moveTo.IsHand, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
            var shuffle = ShuffleDeck(DecisionMaker, HeroTurnTaker.Deck);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shuffle); } else { this.GameController.ExhaustCoroutine(shuffle); }
        }
    }
}
