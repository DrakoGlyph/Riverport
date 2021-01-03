using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class HealthyTreatCardController : TreatCardController
    {
        public HealthyTreatCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            EffectType = SelectionType.GainHP;
        }

        protected override IEnumerator Effect(Card c)
        {
            var heal = this.GameController.GainHP(c, 2);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
        }
    }
}
