using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class GravityWellCardController : EnchantmentBaseCardController
    {
        public GravityWellCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected override TriggerType TriggerType
        {
            get { return TriggerType.ReduceDamageOneUse; }
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddIncreaseDamageTrigger(dda => dda.Target == Enchanted, 1);
        }

        protected override bool DetermineEligible(Card c)
        {
            return !c.IsHero && c.IsTarget;
        }

        protected override IEnumerator WhenDestroyed(DestroyCardAction dca)
        {
            if (IsThisCardNextToCard(Enchanted))
            {
                ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
                rdse.NumberOfUses = 1;
                rdse.SourceCriteria.IsSpecificCard = Enchanted;
                rdse.CreateImplicitExpiryConditions();
                rdse.TargetLeavesPlayExpiryCriteria.Card = Enchanted;
                var status = AddStatusEffect(rdse);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }
            }
        }
    }
}
