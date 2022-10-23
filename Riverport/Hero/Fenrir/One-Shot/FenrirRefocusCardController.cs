using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirRefocusCardController : BaseFenrirCardController
    {
        public FenrirRefocusCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, false)
        {
        }

        public override IEnumerator Play()
        {
            /*
             * You may discard any number of cards.
             * Detransform.
             * Draw cards until you have 4 in hand.
             */
            List<DiscardCardAction> discarded = new List<DiscardCardAction>();
            IEnumerator e = this.GameController.SelectAndDiscardCards(HeroTurnTakerController, null, true, null, discarded, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if (IsWolf)
            {
                e = Detransform();
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            if (HeroTurnTaker.NumberOfCardsInHand < 4)
            {
                e = DrawCardsUntilHandSizeReached(HeroTurnTakerController, 4);
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }

        }
    }
}
