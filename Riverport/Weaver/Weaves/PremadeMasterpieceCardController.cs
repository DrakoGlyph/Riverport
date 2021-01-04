using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class PremadeMasterpieceCardController : CardController
    {
        public PremadeMasterpieceCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var suitUp = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("suit")), false, true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(suitUp); } else { this.GameController.ExhaustCoroutine(suitUp); }
            var material = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("material")), false, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(material); } else { this.GameController.ExhaustCoroutine(material); }
            var play = SelectAndPlayCardFromHand(HeroTurnTakerController, associateCardSource: true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
