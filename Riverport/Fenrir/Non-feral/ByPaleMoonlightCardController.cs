using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class ByPaleMoonlightCardController : FenrirBaseCardController
    {
        public ByPaleMoonlightCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowIfSpecificCardIsInPlay("PaleMoonPendant");
        }
        public override IEnumerator Play()
        {
            if(!FindCard("PaleMoonPendant").IsInPlay)
            {
                var search = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(FindCard("PaleMoonPendant")), true, false, false);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            }
            if(ShouldActivate("human"))
            {
                var heal = this.GameController.GainHP(CharacterCard, 2, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
            }
            if(ShouldActivate("wolf"))
            {
                IncreaseDamageStatusEffect boost = new IncreaseDamageStatusEffect(1);
                boost.SourceCriteria.IsSpecificCard = CharacterCard;
                boost.SourceCriteria.HasIdentifierOfSpecifiedCard = true;
                boost.NumberOfUses = 1;
                boost.TargetCriteria.IsHero = false;
                var effect = AddStatusEffect(boost);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(effect); } else { this.GameController.ExhaustCoroutine(effect); }
            }
        }
    }
}
