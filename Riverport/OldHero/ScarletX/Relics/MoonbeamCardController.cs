using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class MoonbeamCardController : CardController
    {
        public MoonbeamCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(CharacterCard) && dda.DamageType == DamageType.Projectile, 1);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            IEnumerable arrows = FindCardsWhere(c => c.IsInPlayAndNotUnderCard && c.DoKeywordsContain("arrow"), true, GetCardSource());
            var des = this.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria(c => c.IsInPlayAndNotUnderCard && c.DoKeywordsContain("arrow")), false, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(des); } else { this.GameController.ExhaustCoroutine(des); }
        }
    }
}
