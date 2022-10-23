using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirBrainAndBrawnCardController : BaseFenrirCardController
    {
        public FenrirBrainAndBrawnCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override IEnumerator Play()
        {
            /*
             * Human: You may destroy a Villain Ongoing. If you do not, draw a card.
             */
            IEnumerator e;
            if(IsHuman)
            {
                List<DestroyCardAction> destroy = new List<DestroyCardAction>();
                e = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsVillain && c.IsOngoing, "Villain Ongoing"), true,destroy, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                if(!DidDestroyCard(destroy))
                {
                    e = DrawCard();
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                }
            }
            if(IsWolf)
            {
                e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 4, DamageType.Melee, 1, false, 1, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
