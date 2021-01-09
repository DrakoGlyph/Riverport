using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class TameYourselfCardController : FenrirBaseCardController
    {
        public TameYourselfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                var search = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(c => c == FindCard("Meditate")), true, false, false, true);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            }
            if(ShouldActivate("wolf"))
            {
                List<DestroyCardAction> destroyed = new List<DestroyCardAction>();
                int under = 0;
                if (Frenzy.IsInPlayAndHasGameText)
                {
                    var clearFrenzy = this.GameController.DestroyCards(HeroTurnTakerController, new LinqCardCriteria(c => c.Location == Frenzy.UnderLocation), true, destroyed, false, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { this.GameController.StartCoroutine(clearFrenzy); } else { this.GameController.ExhaustCoroutine(clearFrenzy); }
                    under += GetNumberOfCardsDestroyed(destroyed);
                    destroyed.Clear();
                    var destroyFrenzy = this.GameController.DestroyCard(HeroTurnTakerController, Frenzy, false, destroyed, "Fenrir's Frenzy was tamed.", true, null, Card, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { this.GameController.StartCoroutine(destroyFrenzy); } else { this.GameController.ExhaustCoroutine(clearFrenzy); }
                    under += GetNumberOfCardsDestroyed(destroyed);
                }
                if(under>0)
                {
                    var heal = this.GameController.GainHP(CharacterCard, under, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
                } else
                {
                    var des = this.GameController.DestroyCard(HeroTurnTakerController, LycanForm, false, null, "The Wolf is tamed.", true, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(des); } else { this.GameController.ExhaustCoroutine(des); }
                }

            }
        }
    }
}
