using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class ThornsCardController : EnchantmentBaseCardController
    {
        public ThornsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddCounterDamageTrigger(dda => dda.Target == Enchanted && !dda.DamageSource.IsHero, () => Enchanted, () => Enchanted, false, 2, DamageType.Energy);
        }

        protected override TriggerType TriggerType
        {
            get { return TriggerType.DealDamage; }
        }

        protected override bool DetermineEligible(Card c)
        {
            return c.IsHero && c.IsTarget;
        }

        protected override IEnumerator WhenDestroyed(DestroyCardAction dca)
        {
            HeroTurnTaker htt = Enchanted.Owner as HeroTurnTaker;
            var damage = this.GameController.SelectTargetsAndDealDamage(FindHeroTurnTakerController(htt), new DamageSource(GameController, Enchanted), 2, DamageType.Energy, 1, false, 0, additionalCriteria: c => !c.IsHero, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
        }
    }
}
