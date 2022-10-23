using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class BetweenTheShotsCardController : CardController
    {
        public BetweenTheShotsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Breathe, TriggerType.GainHP);
        }

        private IEnumerator Breathe(PhaseChangeAction arg)
        {
            if(!HasBeenDestroyedThisTurn(c=>c.Owner == TurnTaker))
            {
                var heal = this.GameController.GainHP(CharacterCard, 2, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
            }
        }
    }
}
