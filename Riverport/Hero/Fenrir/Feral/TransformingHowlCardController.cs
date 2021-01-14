using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class TransformingHowlCardController : FenrirBaseCardController
    {
        public TransformingHowlCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {

        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                var play = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(LycanForm), true, false, false);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
            }
            if(ShouldActivate("wolf"))
            {
                var howl = DealDamage(CharacterCard, c => !c.IsHero, 1, DamageType.Sonic);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(howl); } else { this.GameController.ExhaustCoroutine(howl); }
            }
        }
    }
}
