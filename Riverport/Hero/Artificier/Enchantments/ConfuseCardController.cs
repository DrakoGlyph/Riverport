using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class ConfuseCardController : EnchantmentBaseCardController
    {
        public ConfuseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override TriggerType TriggerType
        {
            get { return TriggerType.DealDamage; }
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddRedirectDamageTrigger(dda => dda.DamageSource.IsSameCard(Enchanted), () => Enchanted.Owner.CharacterCard);
        }


        protected override bool DetermineEligible(Card c)
        {
            return !c.IsCharacter && c.IsVillainTarget;
        }

        protected override IEnumerator WhenDestroyed(DestroyCardAction dca)
        {
            if (IsThisCardNextToCard(Enchanted))
            {
                var hitSelf = DealDamage(Enchanted, Enchanted, 2, DamageType.Psychic, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(hitSelf); } else { this.GameController.ExhaustCoroutine(hitSelf); }
            }
        }
    }
}
