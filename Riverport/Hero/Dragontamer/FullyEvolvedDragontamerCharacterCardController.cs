using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class FullyEvolvedDragontamerCharacterCardController : HeroCharacterCardController
    {
        public FullyEvolvedDragontamerCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(Card.UnderLocation);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            int damage = GetPowerNumeral(1, 2) + Card.UnderLocation.NumberOfCards;
            int target = GetPowerNumeral(0, 1);
            var slash = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, CharacterCard), damage, DamageType.Melee, target, false, 0, false, additionalCriteria: c => !c.IsHero, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(slash); } else { this.GameController.ExhaustCoroutine(slash); }
            var destroy = this.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria(c => c.Location == Card.UnderLocation), true, null, false, null, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        var pow = this.GameController.SelectHeroToUsePower(DecisionMaker, false, true, false, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pow); } else { this.GameController.ExhaustCoroutine(pow); }
                        break;
                    }
                case 1:
                    {
                        var draw = this.GameController.SelectHeroToDrawCard(DecisionMaker, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                        break;
                    }
                case 2:
                    {
                        var rend = this.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(c => c.IsOngoing && c.IsVillain), false);
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
                        break;
                    }
            }
        }
    }
}
