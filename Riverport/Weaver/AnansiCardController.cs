using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class AnansiCardController : CardController
    {
        public AnansiCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        
        protected readonly LinqCardCriteria SuitMaterialFilter = new LinqCardCriteria(c => c.DoKeywordsContain("suit") || c.DoKeywordsContain("material"), "Suits or Materials", false, false, "Suit or Material", "Suits or Materials");


        public override void AddTriggers()
        {
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Card, c => c == Card, TargetType.All, 2, DamageType.Psychic);
        }

        public override IEnumerator Play()
        {
            MakeIndestructibleStatusEffect mise = new MakeIndestructibleStatusEffect();
            mise.CreateImplicitExpiryConditions();
            mise.CardDestroyedExpiryCriteria.Card = Card;
            mise.CardsToMakeIndestructible.HasAnyOfTheseKeywords = new List<string>() { "suit" };
            var protect = AddStatusEffect(mise);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(protect); } else { this.GameController.ExhaustCoroutine(protect); }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var craft = SearchForCards(HeroTurnTakerController, true, false, 1, 1, SuitMaterialFilter, true, false, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(craft); } else { this.GameController.ExhaustCoroutine(craft); }
            var pay = DealDamage(Card, Card, 2, DamageType.Psychic);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
        }
    }
}
