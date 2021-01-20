using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.ScarletX
{
    public class DebilitatingArrowCardController : ArrowBaseCardController
    {
        public DebilitatingArrowCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            Trigger = TriggerType.DealDamage;
        }

        protected override IEnumerator FireArrow(DestroyCardAction dca = null)
        {
            var damage = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, CharacterCard), 1, DamageType.Projectile, 1, false, 0, false, false, false, c => !c.IsHero, addStatusEffect: Debilitate, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
        }

        private IEnumerator Debilitate(DealDamageAction arg)
        {
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.UntilStartOfNextTurn(TurnTaker);
            rdse.SourceCriteria.IsSpecificCard = arg.Target;
            var stat = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(stat); } else { this.GameController.ExhaustCoroutine(stat); }
        }
    }
}
