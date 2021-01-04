using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class AkashaweaveCardController : MaterialCardController
    {
        public AkashaweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddEndOfTurnTrigger(tt => tt == EquippedTurnTaker, Regen, TriggerType.GainHP);
        }

        private IEnumerator Regen(PhaseChangeAction arg)
        {
            var regen = this.GameController.GainHP(Equipped, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(regen); } else { this.GameController.ExhaustCoroutine(regen); }
        }
    }
}
