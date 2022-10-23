using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ArkHive
{
    public class KnowledgeAndPowerCardController : ArkhivePlanBaseCardController
    {
        public KnowledgeAndPowerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DiscardCardAction>((DiscardCardAction dca) => dca.CardToDiscard.IsHero, EnactTrigger, TriggerType.ActivateTriggers, TriggerTiming.After);

        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                foreach(HeroTurnTaker htt in ArkHiveAndTheSparesTurnTakers)
                {
                    Console.WriteLine(htt.Name);
                    var draw = this.GameController.SelectHeroToDrawCard(FindHeroTurnTakerController(htt), false, true, true, null, new LinqTurnTakerCriteria(tt => tt == htt), 1, GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                }
            }
        }
    }
}
