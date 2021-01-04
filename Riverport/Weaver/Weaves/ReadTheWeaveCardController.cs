using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class ReadTheWeaveCardController : CardController
    {
        public ReadTheWeaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowIfSpecificCardIsInPlay("BookOfFate");
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => true, ReadTheWeave, TriggerType.RevealCard);
            AddStartOfTurnTrigger(tt => tt == TurnTaker, DestroyThisCardResponse, TriggerType.DestroySelf);
        }

        private IEnumerator ReadTheWeave(PhaseChangeAction arg)
        {
            int sight = 2;
            if (FindCard("BookOfFate").IsInPlayAndNotUnderCard) sight += 2;
            var read = RevealCardsFromTopOfDeck_PutOnTopAndOnBottom(HeroTurnTakerController, TurnTakerController, arg.ToPhase.TurnTaker.Deck, sight, sight, 0);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(read); } else { this.GameController.ExhaustCoroutine(read); }
        }
    }
}
