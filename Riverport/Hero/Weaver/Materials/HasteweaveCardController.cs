using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class HasteweaveCardController : MaterialCardController
    {
        public HasteweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddAdditionalPhaseActionTrigger(tt => tt == EquippedTurnTaker, Phase.PlayCard, 1);
        }

        public override IEnumerator Play()
        {
            yield return base.Play();

            var haste = IncreasePhaseActionCountIfInPhase(tt => tt == EquippedTurnTaker, Phase.PlayCard, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(haste); } else { this.GameController.ExhaustCoroutine(haste); }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsPlayCard && this.GameController.ActiveTurnTaker == EquippedTurnTaker;
        }
    }
}
