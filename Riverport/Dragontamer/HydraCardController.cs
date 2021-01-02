using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class HydraCardController : DragonCardController
    {
        public HydraCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            // If there is are cards under this one, destroy one and Hydra regains 1 HP
            AddStartOfTurnTrigger(tt => (Card.MaximumHitPoints > Card.HitPoints) && HasCardsUnder && tt == TurnTaker, Regen, TriggerType.GainHP);
            //When Hydra deals Melee damage, she also deals that target 1 Fire Damage
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.DamageSource.IsSameCard(Card) && dda.DamageType == DamageType.Melee && dda.DidDealDamage, Burn, TriggerType.DealDamage, TriggerTiming.After);

        }

        private IEnumerator Burn(DealDamageAction arg)
        {
            var burn = DealDamage(Card, arg.Target, 1, DamageType.Fire, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
        }

        private IEnumerator Regen(PhaseChangeAction arg)
        {
            //Destroy a card under this one
            var pay = DestroyCardUnderThis();
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
            //Hydra regains 1 HP
            var heal = this.GameController.GainHP(Card, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Hydra deals up to 3 targets 1 Melee damage
            int a = GetPowerNumeral(0, 3);
            int b = GetPowerNumeral(1, 1);

            var bite = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), a, DamageType.Melee, b, false, 0, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(bite); } else { this.GameController.ExhaustCoroutine(bite); }
        }
    }
}
