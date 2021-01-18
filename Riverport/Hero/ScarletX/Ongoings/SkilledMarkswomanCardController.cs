using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class SkilledMarkswomanCardController : CardController
    {
        public SkilledMarkswomanCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return GameController.ActiveTurnPhase.IsPlayCard && GameController.ActiveTurnTaker == TurnTaker;
        }

        public override IEnumerator Play()
        {
            var play = IncreasePhaseActionCountIfInPhase(tt => tt == TurnTaker, Phase.PlayCard, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }

        public override void AddTriggers()
        {
            AddAdditionalPhaseActionTrigger(tt => tt == TurnTaker, Phase.PlayCard, 1);
        }
    }
}
