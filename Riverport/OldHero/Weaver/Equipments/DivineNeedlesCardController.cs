using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class DivineNeedlesCardController : CardController
    {
        public DivineNeedlesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(CharacterCard) && dda.DamageType == DamageType.Projectile, 1);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var needle = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, CharacterCard), 1, DamageType.Projectile, 3, false, 0, true, addStatusEffect: Temper, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(needle); } else { this.GameController.ExhaustCoroutine(needle); }
        }

        private IEnumerator Temper(DealDamageAction arg)
        {
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.UntilStartOfNextTurn(TurnTaker);
            rdse.SourceCriteria.IsSpecificCard = arg.Target;
            var temper = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(temper); } else { this.GameController.ExhaustCoroutine(temper); }
        }
    }
}
