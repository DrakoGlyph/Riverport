using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class ResearchHandlerCardController : CardController
    {
        public ResearchHandlerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowLowestHP(cardCriteria: new LinqCardCriteria(c => c.DoKeywordsContain("dragon")));
        }

        public override void AddTriggers()
        {
            AddDealDamageAtEndOfTurnTrigger(TurnTaker, Card, c => c.DoKeywordsContain("dragon"), TargetType.LowestHP, 1, DamageType.Lightning);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.DamageSource.IsSameCard(Card), Inhibit, TriggerType.ReduceDamage, TriggerTiming.After);
        }

        private IEnumerator Inhibit(DealDamageAction arg)
        {
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.UntilStartOfNextTurn(TurnTaker);
            rdse.TargetCriteria.IsSpecificCard = arg.Target;
            var status = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }
        }
    }
}
