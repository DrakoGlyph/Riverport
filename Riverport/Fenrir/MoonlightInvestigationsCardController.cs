using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class MoonlightInvestigationsCardController : FenrirBaseCardController
    {
        public MoonlightInvestigationsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override void AddTriggers()
        {
            AddAdditionalPhaseActionTrigger(tt => tt == TurnTaker, Phase.DrawCard, 1);
        }

        public override IEnumerator Play()
        {
            var boost = IncreasePhaseActionCountIfInPhase(tt => tt == TurnTaker, Phase.DrawCard, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(boost); } else { this.GameController.ExhaustCoroutine(boost); }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var search = SearchForCards(HeroTurnTakerController, true, false, 1, 1, new LinqCardCriteria(c => !c.DoKeywordsContain("feral")), false, true, false, true, shuffleAfterwards: true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }

            List<YesNoCardDecision> temp = new List<YesNoCardDecision>();
            var investigate = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.DestroySelf, Card, null, temp, null, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(investigate); } else { this.GameController.ExhaustCoroutine(investigate); }
            if (DidPlayerAnswerYes(temp))
            {
                var exhaust = this.GameController.DestroyCard(HeroTurnTakerController, Card, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(exhaust); } else { this.GameController.ExhaustCoroutine(exhaust); }

                var play = SelectAndPlayCardFromHand(HeroTurnTakerController);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
            }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsDrawCard && this.GameController.ActiveTurnTaker == TurnTaker;
        }
    }
}
