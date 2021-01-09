using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class GeekAndSunderCardController : FenrirBaseCardController
    {
        public GeekAndSunderCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("Human"))
            {
                var geek = DrawCards(HeroTurnTakerController, 2);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(geek); } else { this.GameController.ExhaustCoroutine(geek); }
            }
            if(ShouldActivate("Wolf"))
            {
                List<DestroyCardAction> storedResultsAction = new List<DestroyCardAction> { };
                var sunder = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsOngoing || c.IsDevice), false, storedResultsAction, CharacterCard, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(sunder); } else { this.GameController.ExhaustCoroutine(sunder); }
                var rend = this.GameController.DealDamageToTarget(new DamageSource(GameController, CharacterCard), GetDestroyedCards(storedResultsAction).FirstOrDefault(), 2, DamageType.Melee, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
            }
        }
    }
}
