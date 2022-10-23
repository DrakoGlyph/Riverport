using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirHackAndSlashCardController : BaseFenrirCardController
    {
        public FenrirHackAndSlashCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override IEnumerator Play()
        {
            /*
             * Human: Reveal the top three cards of your deck. You may put 1 into your hand. Shuffle the rest back into your deck.
             * Wolf: Fenrir deals up to 3 targets 2 Melee damage each
             */
            IEnumerator e;
            if(IsHuman)
            {
                List<Card> revealed = new List<Card>();
                e = this.GameController.RevealCards(TurnTakerController, HeroTurnTaker.Deck, 3, revealed, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                e = this.GameController.SelectCardFromLocationAndMoveIt(HeroTurnTakerController, HeroTurnTaker.Revealed, new LinqCardCriteria(), new List<MoveCardDestination> { new MoveCardDestination(HeroTurnTaker.Hand) }, optional: true, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                e = CleanupRevealedCards(HeroTurnTaker.Revealed, HeroTurnTaker.Hand);
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            if(IsWolf)
            {
                e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 2, DamageType.Melee, 3, true, null, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
