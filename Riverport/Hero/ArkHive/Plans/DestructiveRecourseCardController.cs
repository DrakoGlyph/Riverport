using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace Riverport.ArkHive
{
    public class DestructiveRecourseCardController : ArkhivePlanBaseCardController
    {
        public DestructiveRecourseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.ResponsibleCard.IsVillainCharacterCard && dca.CardToDestroy.Card.IsHero && (dca.CardToDestroy.Card.DoKeywordsContain("ongoing") || dca.CardToDestroy.Card.DoKeywordsContain("equipment")) && dca.CardToDestroy.Card != Card, EnactTrigger, TriggerType.ActivateTriggers, TriggerTiming.After);
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                var destroy = this.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(c => (c.IsOngoing) && c.IsVillain), false);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }
    }
}
