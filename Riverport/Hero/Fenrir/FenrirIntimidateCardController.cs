using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirIntimidateCardController : BaseFenrirCardController
    {
        public FenrirIntimidateCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override IEnumerator Play()
        {
            /*
             * Human: Fenrir deals 1 target 2 Psychic damage. If this damage destroys the target, you may draw a card.
             * Wolf: Fenrir deals each Non-Hero target 1 Psychic damage. Targets dealt damage this way deal 1 less damage until the start of Fenrir's next turn.
             */
            IEnumerator e;
            if(IsHuman)
            {
                List<DealDamageAction> damage = new List<DealDamageAction>();
                e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 2, DamageType.Psychic, 1, false, 1, storedResultsDamage: damage, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
                DealDamageAction dda = damage.FirstOrDefault();
                if(dda.DidDestroyTarget)
                {
                    e = DrawCard(optional: true);
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                }
            }
            if(IsWolf)
            {
                ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
                rdse.UntilStartOfNextTurn(TurnTaker);
                e = DealDamage(CharacterCard, c => c.IsTarget && !c.IsHero, 1, DamageType.Psychic, addStatusEffect: dda => this.GameController.AddStatusEffect(rdse, false, GetCardSource()));
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
