using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class ScarletXCharacterCardController : HeroCharacterCardController
    {
        public ScarletXCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var shoot = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")), false, null, CharacterCard, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shoot); } else { this.GameController.ExhaustCoroutine(shoot); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        var power = this.GameController.SelectHeroToUsePower(DecisionMaker, false, true, true, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(power); } else { this.GameController.ExhaustCoroutine(power); }
                        break;
                    }
                case 1:
                    {
                        List<DestroyCardAction> results = new List<DestroyCardAction>();
                        var dest = this.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(c => c.IsHero && c.IsInPlayAndHasGameText && IsEquipment(c)), true, results, CharacterCard, GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(dest); } else { this.GameController.ExhaustCoroutine(dest); }
                        if (DidDestroyCard(results))
                        {
                            TurnTaker tt = GetDestroyedCards(results).FirstOrDefault().Owner;
                            if (tt is HeroTurnTaker taker)
                            {
                                HeroTurnTakerController httc = FindHeroTurnTakerController(taker);
                                var damage = this.GameController.SelectTargetsAndDealDamage(httc, new DamageSource(GameController, httc.CharacterCard), 2, DamageType.Projectile, 1, false, 0, false, true, false, IsVillainTarget, cardSource: GetCardSource());
                                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        var draw = this.GameController.SelectHeroToDrawCard(DecisionMaker, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                        break;
                    }
            }
        }
    }
}
