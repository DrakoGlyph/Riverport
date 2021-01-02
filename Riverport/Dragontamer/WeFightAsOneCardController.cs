using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;

namespace Riverport.Dragontamer
{
    public class WeFightAsOneCardController : CommandCardController
    {
        public WeFightAsOneCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            for(int i = 0; i < CommandPresence; i++) {
                var command = SelectAndUsePower(this, true, p => p.CardController is DragonCardController);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(command); } else { this.GameController.ExhaustCoroutine(command); }
            }
            var empower = SelectDragonToMoveUnder();
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(empower); } else { this.GameController.ExhaustCoroutine(empower); }
        }

    }
}
