using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class ElgorCardController : DragonCardController
    {
        public ElgorCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>((DealDamageAction dda) => HasCardsUnder &&  dda.DidDealDamage && dda.Target.IsCharacter && dda.DamageSource.IsSameCard(Card), Sap, TriggerType.DestroyCard, TriggerTiming.After, requireActionSuccess: true);
        }

        private IEnumerator Sap(DealDamageAction arg)
        {
            List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
            var decide = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.MoveCardFromUnderCard, Card, null, decision, null, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            if(DidPlayerAnswerYes(decision))
            {
                TurnTaker owner = arg.Target.Owner;
                var destroy = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => (c.IsOngoing || c.IsDevice) && !(c.IsCharacter) && c.Location == owner.PlayArea), false, null, Card, GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            int targets = GetPowerNumeral(0, 1);
            int damage = GetPowerNumeral(1, 2);
            var burn = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), damage, DamageType.Fire, targets, false, 0, additionalCriteria: c => !c.IsHero, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
        }
    }
}
