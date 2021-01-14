using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class CallOnTheLightCardController : CardController
    {
        public CallOnTheLightCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var moon = SearchForCards(DecisionMaker, true, true, 1, 1, new LinqCardCriteria(FindCard("Moonbeam")), true, false, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(moon); } else { this.GameController.ExhaustCoroutine(moon); }
            var arrow = SelectAndPlayCardFromHand(DecisionMaker, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")), false, true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(arrow); } else { this.GameController.ExhaustCoroutine(arrow); }
        }
    }
}
