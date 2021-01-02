using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class ProteinSnackCardController : TreatCardController
    {
        public ProteinSnackCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            EffectType = SelectionType.IncreaseDamage;
        }

        protected override IEnumerator Effect(Card c)
        {
            IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
            idse.SourceCriteria.IsSpecificCard = c;
            idse.NumberOfUses = 1;
            var effect = AddStatusEffect(idse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(effect); } else { this.GameController.ExhaustCoroutine(effect); }
        }
    }
}
