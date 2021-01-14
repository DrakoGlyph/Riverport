using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Weaver
{
    public class PsychweaveCardController : MaterialCardController
    {
        public PsychweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddAdditionalPhaseActionTrigger(tt => tt == EquippedTurnTaker, Phase.DrawCard, 1);
        }

        public override IEnumerator Play()
        {
            yield return base.Play();

            var psych = IncreasePhaseActionCountIfInPhase(tt => tt == EquippedTurnTaker, Phase.DrawCard, 1);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(psych); } else { this.GameController.ExhaustCoroutine(psych); }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsDrawCard && this.GameController.ActiveTurnTaker == EquippedTurnTaker;
        }
    }
}
