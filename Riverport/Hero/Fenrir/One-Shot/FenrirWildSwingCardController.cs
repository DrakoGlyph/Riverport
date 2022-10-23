using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirWildSwingCardController : BaseFenrirCardController
    {
        public FenrirWildSwingCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override IEnumerator Play()
        {
           /*
            * Human: Reveal the top card of your deck. If it is a One-Shot, put it into play, otherwise put it in your hand.
            * Wolf: Fenrir deals 1 target 5 Melee damage. If this damage destroys the target, Fenrir may deal a second target 3 Melee damage.
            */
            IEnumerator e;
            if (IsHuman)
            {
                List<Card> revealed = new List<Card>();
                e = this.GameController.RevealCards(TurnTakerController, HeroTurnTaker.Deck, 1, revealed, false, RevealedCardDisplay.ShowRevealedCards, null, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                Card c = GetRevealedCard(revealed);
                if (c.IsOneShot) e = this.GameController.PlayCard(TurnTakerController, c, true, cardSource: GetCardSource());
                else e = this.GameController.MoveCard(TurnTakerController, c, HeroTurnTaker.Hand, cardSource: GetCardSource());
                
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                e = CleanupRevealedCards(HeroTurnTaker.Revealed, HeroTurnTaker.Deck);
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            if(IsWolf)
            {
                List<DealDamageAction> damage = new List<DealDamageAction>();
                e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 5, DamageType.Melee, 1, false, 1, storedResultsDamage: damage, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);

                if(damage.FirstOrDefault().DidDestroyTarget)
                {
                    e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 3, DamageType.Melee, 1, true, null, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                }
            }
        }
    }
}
