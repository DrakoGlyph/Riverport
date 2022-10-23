using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ArkHive
{
    public class ExcessiveForceCardController : ArkhivePlanBaseCardController
    {
        public ExcessiveForceCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.Amount > 5 && dda.Target.IsHero, EnactTrigger, TriggerType.WouldBeDealtDamage, TriggerTiming.Before);
        }

        public override IEnumerator ActivateAbility(string abilityKey)
        {
            if (abilityKey == "enact")
            {
                List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
                var select = this.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.IncreaseDamage, ArkHiveAndTheSparesCards, storedResults, false, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
                IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(2);
                idse.NumberOfUses = 1;
                idse.SourceCriteria.IsSpecificCard = GetSelectedCard(storedResults);
                var effect = AddStatusEffect(idse);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(effect); } else { this.GameController.ExhaustCoroutine(effect); }
            }
        }
    }
}
