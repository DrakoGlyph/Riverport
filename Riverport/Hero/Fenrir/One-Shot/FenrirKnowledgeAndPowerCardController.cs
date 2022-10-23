using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirKnowledgeAndPowerCardController : BaseFenrirCardController
    {
        public FenrirKnowledgeAndPowerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override IEnumerator Play()
        {
            /*
             * Fenrir deals himself up to 3 Infernal Damage. X on this card is equal to the amount of Infernal damage taken this way
             * Human: You may draw up to X cards
             * Wolf: Fenrir either deals X targets 1 Psychic damage or 1 target X Melee damage
             */
            List<SelectNumberDecision> selectNumbers = new List<SelectNumberDecision>();
            IEnumerator e = this.GameController.SelectNumber(HeroTurnTakerController, SelectionType.DealDamageSelf, 0, 3, storedResults: selectNumbers, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            int? x = GetSelectedNumber(selectNumbers);
            List<DealDamageAction> damage = new List<DealDamageAction>();
            e = DealDamage(CharacterCard, CharacterCard, x.Value, DamageType.Infernal, storedResults: damage, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if(IsHuman)
            {
                e = DrawCards(HeroTurnTakerController, x.Value, true, true);
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            if (IsWolf)
            {
                List<Function> functions = new List<Function>()
                {
                new Function(HeroTurnTakerController, "Deal {x} targets 1 Psychic damage", SelectionType.DealDamage, () => this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 1, DamageType.Psychic, x, true, null, cardSource:GetCardSource())),
                new Function(HeroTurnTakerController,"Deal 1 target {x} Melee damage", SelectionType.DealDamage, () => this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, x.Value, DamageType.Melee, 1, true, null, cardSource: GetCardSource()))
                };
                e = SelectAndPerformFunction(HeroTurnTakerController, functions, true);
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
