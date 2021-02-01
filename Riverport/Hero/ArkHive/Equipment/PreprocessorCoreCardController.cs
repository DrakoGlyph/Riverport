using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ArkHive
{
    public class PreprocessorCoreCardController : ArkHiveBaseCardController
    {
        public PreprocessorCoreCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override void AddTriggers()
        {
            AddAdditionalPhaseActionTrigger(tt => tt == TurnTaker, Phase.DrawCard, 1);
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsDrawCard && this.GameController.ActiveTurnTaker == TurnTaker;
        }

        public override IEnumerator Play()
        {
            var increase = IncreasePhaseActionCountIfInPhase(tt => tt == TurnTaker, Phase.DrawCard, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(increase); } else { this.GameController.ExhaustCoroutine(increase); } 
        }
    }
}
