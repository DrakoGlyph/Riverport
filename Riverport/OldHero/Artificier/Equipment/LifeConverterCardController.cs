using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class LifeConverterCardController : ArtificierBaseCardController
    {
        public LifeConverterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>(dca => dca.CardToDestroy.Card.IsTarget && dca.ResponsibleCard == CharacterCard, dca => this.GameController.AddTokensToPool(ManaPool, 1, GetCardSource()), TriggerType.AddTokensToPool, TriggerTiming.After);
            AddStartOfTurnTrigger(tt => tt == TurnTaker, ConvertLife, TriggerType.DealDamage);
        }

        private IEnumerator ConvertLife(PhaseChangeAction arg)
        {
            List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
            var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.DealDamageSelf, Card, null, decision, null, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            if (DidPlayerAnswerYes(decision))
            {
                var cut = DealDamage(CharacterCard, CharacterCard, 2, DamageType.Energy, false, false, false, dda => this.GameController.AddTokensToPool(ManaPool, dda.Amount, GetCardSource()), cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(cut); } else { this.GameController.ExhaustCoroutine(cut); }
            }
        }
    }
}
