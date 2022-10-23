using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class TortugaCardController : DragonCardController
    {
        public TortugaCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddReduceDamageTrigger(c => c == this.Card, 1);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => HasCardsUnder && (dda.Target.DoKeywordsContain("dragon") || dda.Target == CharacterCard), Protect, TriggerType.WouldBeDealtDamage, TriggerTiming.Before, orderMatters: true, priority: TriggerPriority.High);
        }

        private IEnumerator Protect(DealDamageAction arg)
        {
            List<YesNoCardDecision> cardDecisions = new List<YesNoCardDecision>();
            var doIt = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.MoveCardFromUnderCard, Card, arg, cardDecisions, null, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(doIt); } else { this.GameController.ExhaustCoroutine(doIt); }
            if (DidPlayerAnswerYes(cardDecisions))
            {
                var redirect = RedirectDamage(arg, TargetType.SelectTarget, c => c == Card);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(redirect); } else { this.GameController.ExhaustCoroutine(redirect); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.TargetCriteria.IsOneOfTheseCards = (List<Card>) FindCardsWhere(c=> c==CharacterCard || (c.DoKeywordsContain("dragon") && c.Owner == TurnTaker));
            rdse.UntilStartOfNextTurn(TurnTaker);
            var dragons = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(dragons); } else { this.GameController.ExhaustCoroutine(dragons); }
        }
    }
}
