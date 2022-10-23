using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class AnticipatedCombatCardController : ArkhivePlanBaseCardController
    {
        public AnticipatedCombatCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt.IsVillain, EnactTrigger, TriggerType.FirstTrigger);
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(2);
                rdse.UntilStartOfNextTurn(TurnTaker);
                rdse.TargetCriteria.IsOneOfTheseCards = ArkHiveAndTheSparesCards;
                var anticipate = AddStatusEffect(rdse); 
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(anticipate); } else { this.GameController.ExhaustCoroutine(anticipate); }
            }
        }
    }
}
