using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class SpoolTheThreadCardController : CardController
    {
        public SpoolTheThreadCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var draw = DrawCards(HeroTurnTakerController, 4);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
            var discard = SelectAndDiscardCards(HeroTurnTakerController, 2);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(discard); } else { this.GameController.ExhaustCoroutine(discard); }
        }
    }
}
