using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class GaiaPatchCardController : PatchCardController
    {
        public GaiaPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override TriggerType TriggerType { get { return TriggerType.GainHP; } }

        protected override IEnumerator Empower(GameAction ga = null)
        {
            var heal = this.GameController.GainHP(Equipped, 3, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
        }
    }
}
