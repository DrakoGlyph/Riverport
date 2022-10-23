using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirHonedDisciplineCardController : BaseFenrirCardController
    {
        public FenrirHonedDisciplineCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, false)
        {
        }

        public override void AddTriggers()
        {
            /*
             * When Fenrir takes Infernal damage, increase the next damage dealt by Fenrir to a Non-Hero target by 1
             * At the start of your turn, Fenrir may deal himself 1 Infernal damage. If Fenrir does not take damage this way, destroy this card.
             */
            
            AddTrigger<DealDamageAction>(dda => dda.Target == Card && dda.DamageType == DamageType.Infernal, InfernalBoost, TriggerType.IncreaseDamage, TriggerTiming.After);
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Discipline, TriggerType.DealDamage);
        }

        private IEnumerator Discipline(PhaseChangeAction pca)
        {
            List<YesNoCardDecision> decide = new List<YesNoCardDecision>();
            var e = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.DealDamageSelf, Card, pca, decide, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if (DidPlayerAnswerYes(decide))
            {
                e = DealDamage(CharacterCard, CharacterCard, 1, DamageType.Infernal, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }

        private IEnumerator InfernalBoost(DealDamageAction dda)
        {
            IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(dda.Amount);
            idse.NumberOfUses = 1;
            idse.TargetCriteria.IsHero = false;

            var e = this.GameController.AddStatusEffect(idse, true, GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            IEnumerator e;
            switch(index)
            {
                case 0:
                    // Fenrir deals himself 1 Infernal damage, then deals 1 Non-Hero target 2 Melee damage
                    e = DealDamage(CharacterCard, CharacterCard, 1, DamageType.Infernal, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 2, DamageType.Melee, 1, false, 1, additionalCriteria: c => c.IsTarget && !c.IsHero, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
            }
        }
    }
}
