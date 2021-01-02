using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class DrakoCardController : DragonCardController
    {
        public DrakoCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
           
        }

        public override void AddTriggers()
        {
            // At the start of your turn, destroy a card under this one (if there is one)
            AddStartOfTurnTrigger(tt => HasCardsUnder && tt == TurnTaker, DestroyCardUnderThis, TriggerType.DestroyCard);
            // Increase damage by 1 if cards under
            AddIncreaseDamageTrigger(dda => HasCardsUnder && dda.DamageSource.IsSameCard(Card), 1);
            // If Drako reduces a target to 0 HP
            AddTrigger<DealDamageAction>((DealDamageAction dda)=>dda.DamageSource.IsSameCard(Card) && dda.TargetHitPointsAfterBeingDealtDamage <= 0, Consume, TriggerType.MoveCard, TriggerTiming.After);
        }

        private IEnumerator Consume(DealDamageAction dda) {
            var eat = this.GameController.MoveCard(TurnTakerController, dda.Target, Card.UnderLocation, doesNotEnterPlay:true, cardSource: GetCardSource());
            if(UseUnityCoroutines) {yield return this.GameController.StartCoroutine(eat);} else {this.GameController.ExhaustCoroutine(eat);}
        }

        public override IEnumerator UsePower(int index = 0)
        {
            // Drako deals 1 Target @ Melee then 2 Fire
            
            int a = GetPowerNumeral(0, 1);
            int b = GetPowerNumeral(1, 2);
            int c = GetPowerNumeral(2, 2);
            List<DealDamageAction> slashBurn = new List<DealDamageAction>() {
                new DealDamageAction(GetCardSource(), new DamageSource(GameController, Card), null, b, DamageType.Melee),
                new DealDamageAction(GetCardSource(), new DamageSource(GameController, Card), null, c, DamageType.Fire)
            };
            var attack = SelectTargetsAndDealMultipleInstancesOfDamage(slashBurn, card => !card.IsHero, null, a, a);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(attack); } else { this.GameController.ExhaustCoroutine(attack); }
        }
    }
}
