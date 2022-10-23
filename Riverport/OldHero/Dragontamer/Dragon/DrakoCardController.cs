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
            AddStartOfTurnTrigger(tt => tt == TurnTaker && HasCardsUnder, Consume, TriggerType.DestroyCard);
        }

        private IEnumerator Consume(PhaseChangeAction arg)
        {
            // Destroy a card under this to increase damage by Drako until the start of your next turn
            List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
            var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.MoveCardFromUnderCard, Card, null, decision, null, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            if(DidPlayerAnswerYes(decision))
            {
                var pay = DestroyCardUnderThis(arg);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                idse.SourceCriteria.IsSpecificCard = Card;
                idse.UntilStartOfNextTurn(TurnTaker);
                var boost = AddStatusEffect(idse);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(boost); } else { this.GameController.ExhaustCoroutine(boost); }
            }
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
