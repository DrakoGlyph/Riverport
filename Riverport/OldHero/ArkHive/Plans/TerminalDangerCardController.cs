using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class TerminalDangerCardController : ArkhivePlanBaseCardController
    {
        public TerminalDangerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.Target.IsHeroCharacterCard && dda.TargetHitPointsAfterBeingDealtDamage <= 5, EnactTrigger, TriggerType.WouldBeDealtDamage, TriggerTiming.Before, priority: TriggerPriority.Medium);
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                var destroy = this.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(c => c.HitPoints <= 5), true, null, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }
    }
}
