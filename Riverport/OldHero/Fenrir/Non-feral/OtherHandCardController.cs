using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class OtherHandCardController : FenrirBaseCardController
    {
        public OtherHandCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowSpecialString(OtherHandString);

            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        private string OtherHandString()
        {
            if(ShouldActivateWolf())
            {
                if(HasBeenSetToTrueThisTurn("OtherHand"))
                {
                    return "Fenrir has dealt Other Hand damage this turn.";
                } else
                {
                    return "Fenrir can deal Other Hand damage this turn.";
                }
            }
            return null;
        }

        public override void AddTriggers()
        {
            //AddStartOfTurnTrigger(tt => true, Reset, TriggerType.Other);
            AddAdditionalPhaseActionTrigger(tt => ShouldActivate("human") && tt == TurnTaker, Phase.PlayCard, 1);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => !IsPropertyTrue("OtherHand") && dda.DidDealDamage && dda.DamageType == DamageType.Melee && IsFenrir(dda.DamageSource.Card), OtherWolfHand, TriggerType.DealDamage, TriggerTiming.After);
            AddAfterLeavesPlayAction((GameAction ga) => ResetFlagAfterLeavesPlay("OtherHand"), TriggerType.Hidden);
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsPlayCard && ShouldActivate("human") && this.GameController.ActiveTurnTaker == TurnTaker;
        }

        private IEnumerator OtherWolfHand(DealDamageAction arg)
        {
            if (ShouldActivate("wolf"))
            {
                SetCardPropertyToTrueIfRealAction("OtherHand");
                var fear = DealDamage(CharacterCard, arg.Target, 1, DamageType.Psychic, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(fear); } else { this.GameController.ExhaustCoroutine(fear); }

            }
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                var plus = IncreasePhaseActionCountIfInPhase(tt => tt == TurnTaker, Phase.PlayCard, 1);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(plus); } else { this.GameController.ExhaustCoroutine(plus); }
            }
        }

        private IEnumerator Reset(PhaseChangeAction arg)
        {
            SetCardProperty("OtherHand", false);
            return DoNothing();
        }


    }
}
