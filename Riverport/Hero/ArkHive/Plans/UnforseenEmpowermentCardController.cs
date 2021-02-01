using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class UnforseenEmpowermentCardController : ArkhivePlanBaseCardController
    {
        public UnforseenEmpowermentCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            //AllowFastCoroutinesDuringPretend = false;
            RunModifyDamageAmountSimulationForThisCard = false;
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>((DealDamageAction dda) => (!dda.DidDealDamage) && dda.DamageSource.IsHero, EnactTrigger, TriggerType.ActivateTriggers, TriggerTiming.After);
            
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                var pow = this.GameController.SelectHeroToUsePower(DecisionMaker, additionalCriteria: new LinqTurnTakerCriteria(tt=> (tt is HeroTurnTaker htt)?FilterTurnTakerByArchiveAndSpares(htt):false), cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pow); } else { this.GameController.ExhaustCoroutine(pow); }
            }
        }
    }
}
