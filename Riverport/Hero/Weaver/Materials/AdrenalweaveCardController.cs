using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class AdrenalweaveCardController : MaterialCardController
    {
        public AdrenalweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddAdditionalPhaseActionTrigger(tt => tt == EquippedTurnTaker, Phase.UsePower, 1);
        }

        public override IEnumerator Play()
        {
            yield return base.Play();

            var adrenaline = IncreasePhaseActionCountIfInPhase(tt => tt == EquippedTurnTaker, Phase.UsePower, 1);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(adrenaline); } else { this.GameController.ExhaustCoroutine(adrenaline); }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsUsePower && this.GameController.ActiveTurnTaker == EquippedTurnTaker;
        }
    }
}
