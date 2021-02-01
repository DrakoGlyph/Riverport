using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class HastenedActionCardController : ArkhivePlanBaseCardController
    {
        public HastenedActionCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<PlayCardAction>((PlayCardAction pca) => !IsFirstTimeCardPlayedThisTurn(pca.CardToPlay, c => c.IsVillain && GameController.ActiveTurnTaker == c.Owner, TriggerTiming.After), EnactTrigger, TriggerType.ActivateTriggers, TriggerTiming.After); 
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                var play = this.GameController.SelectHeroToPlayCard(DecisionMaker, false, true, true, new LinqTurnTakerCriteria(tt => (tt is HeroTurnTaker htt) ? FilterTurnTakerByArchiveAndSpares(htt) : false), cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
            }
        }

    }
}
