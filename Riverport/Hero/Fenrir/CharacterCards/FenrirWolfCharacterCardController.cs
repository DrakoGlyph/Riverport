using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirWolfCharacterCardController : BaseFenrirCharacterCardController
    {
        public FenrirWolfCharacterCardController(Card card, TurnTakerController ttc) : base(card, ttc)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Upkeep, TriggerType.DiscardCard);
        }

        private IEnumerator Upkeep(PhaseChangeAction arg)
        {
            /*
             * At the start of your turn, either discard a card, or Detransform.
             */
            List<DiscardCardAction> discarded = new List<DiscardCardAction>();
            IEnumerator e = SelectAndDiscardCards(HeroTurnTakerController, 1, true, storedResults: discarded, gameAction: arg, extraInfo: () => "Discard a card or Detransform");
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if(GetNumberOfCardsDiscarded(discarded) == 0)
            {
                e = Detransform();
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            IEnumerator e;
            switch (index)
            {
                case 0:
                    /*
                     * Fenrir deals 1 target 2 Melee damage
                     * If that target is destroyed, Fenrir may deal 1 target 2 Psychic damage
                     */

                    List<DealDamageAction> damaged = new List<DealDamageAction>();
                    e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), 2, DamageType.Melee, 1, false, null, storedResultsDamage: damaged, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    DealDamageAction dda = damaged.FirstOrDefault();
                    if (!dda.DidDestroyTarget) break;
                    e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), 2, DamageType.Psychic, 1, true, null, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);

                break;
            }
        }


        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            IEnumerator e = null;
            switch (index)
            {
                case 0:
                    /*
                     * One Hero deals 1 target 2 Psychic damage
                     */
                    e = this.GameController.SelectHeroToSelectTargetAndDealDamage(HeroTurnTakerController, 2, DamageType.Psychic, cardSource: GetCardSource());
                    break;
                case 1:
                    /*
                     * Destroy one target with less than 3 HP
                     */
                    e = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsTarget && c.HitPoints < 3, "targets with less than 3 HP", false), true, responsibleCard: Card, cardSource: GetCardSource());
                    break;
                case 2:
                    /*
                     * Destroy one Villain Ongoing.
                     */
                    e = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsVillain && c.IsOngoing, "Villain ongoing"), true, responsibleCard: Card, cardSource: GetCardSource());
                    break;
            }
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }

    }
}
