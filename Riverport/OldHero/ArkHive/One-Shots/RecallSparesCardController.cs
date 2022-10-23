using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ArkHive
{
    public class RecallSparesCardController : ArkHiveBaseCardController
    {
        public RecallSparesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<MoveCardAction> storedResults = new List<MoveCardAction>();
            var recall = this.GameController.SelectAndReturnCards(DecisionMaker, SpareNanobots, new LinqCardCriteria(c => c.DoKeywordsContain("nanobot")), true, false, true, 0, storedResults, null, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(recall); } else { this.GameController.ExhaustCoroutine(recall); }
            int collected = GetNumberOfCardsMoved(storedResults);
            if(collected > 0)
            {
                var heal = this.GameController.GainHP(CharacterCard, collected * 2, null, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
            }
        }
    }
}
