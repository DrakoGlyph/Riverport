using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class QuickWeavingCardController : CardController
    {
        public QuickWeavingCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var plays = SelectAndPlayCardsFromHand(DecisionMaker, 3, false, 0, new LinqCardCriteria(c => c.DoKeywordsContain("patch")));
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(plays); } else { this.GameController.ExhaustCoroutine(plays); } 
        }
    }
}
