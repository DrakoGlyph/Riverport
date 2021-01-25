using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class HungerOfTheWolfCardController : FenrirBaseCardController
    {
        public HungerOfTheWolfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowIfSpecificCardIsInPlay("LycanForm");
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Hunger, TriggerType.PlayCard);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => IsFenrir(dda.DamageSource.Card) && dda.TargetHitPointsAfterBeingDealtDamage <= 0, Consume, TriggerType.GainHP, TriggerTiming.After);
        }

        private IEnumerator Consume(DealDamageAction arg)
        {
            if(ShouldActivate("wolf"))
            {
                var heal = this.GameController.GainHP(CharacterCard, 1, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
            }
        }

        private IEnumerator Hunger(PhaseChangeAction arg)
        {
            if (ShouldActivate("human"))
            {
                List<YesNoCardDecision> doIt = new List<YesNoCardDecision> { };
                var decide = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.DealDamageSelf, Card, null, doIt, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(doIt))
                {
                    var pay = DealDamage(CharacterCard, CharacterCard, 1, DamageType.Psychic, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                    var transform = this.GameController.PlayCard(TurnTakerController, LycanForm, true, associateCardSource: true, cardSource: GetCardSource());
                    //var transform = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(LycanForm), true, false, false);
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(transform); } else { this.GameController.ExhaustCoroutine(transform); }
                }
            }
        }
    }
}
