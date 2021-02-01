using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class RegenerativeOptionCardController : ArkhivePlanBaseCardController
    {
        public RegenerativeOptionCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<GainHPAction>((GainHPAction ghpa) => ghpa.HpGainer.IsVillainCharacterCard, EnactTrigger, TriggerType.ActivateTriggers, TriggerTiming.Before);
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if(abilityKey == "enact")
            {
                foreach(Card c in ArkHiveAndTheSparesCards)
                {
                    var heal = this.GameController.GainHP(c, 2, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
                }
            }
        }
    }
}
