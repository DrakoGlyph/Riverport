using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class DragontamerCharacterCardController : CharacterCardController
    {
        public DragontamerCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            // Dragontamer deals 1 target 1 Projectile Damage
            // Increase damage dealt by dragons to that target by 1 until the start of your next turn
            int a = GetPowerNumeral(0, 1);
            int b = GetPowerNumeral(1, 1);
            var shoot = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), b, DamageType.Projectile, a, false, 0, false, addStatusEffect: Mark, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shoot); } else { this.GameController.ExhaustCoroutine(shoot); }
            
        }

        private IEnumerator Mark(DealDamageAction arg)
        {
            // Dragons deal 1 more damage to target
            IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
            idse.SourceCriteria.HasAnyOfTheseKeywords = new List<string>() { "dragon" };
            idse.TargetCriteria.IsSpecificCard = arg.Target;
            idse.UntilStartOfNextTurn(TurnTaker);
            var mark = AddStatusEffect(idse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(mark); } else { this.GameController.ExhaustCoroutine(mark); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        //Destroy a Device
                        var rend = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsDevice), false, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
                        break;
                    }
                case 1:
                    {
                        //One Hero may Use a Power now
                        var pow = this.GameController.SelectHeroToUsePower(HeroTurnTakerController, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pow); } else { this.GameController.ExhaustCoroutine(pow);  }
                        break;
                    }
                case 2:
                    {
                        //Increase Hero Damage by 1 until the start of your next turn
                        IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                        idse.SourceCriteria.IsHero = true;
                        idse.UntilStartOfNextTurn(TurnTaker);
                        var bold = AddStatusEffect(idse);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(bold); } else { this.GameController.ExhaustCoroutine(bold); }
                        break;
                    }
            }
        }
    }
}
