using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class UnleashYourselfCardController : FenrirBaseCardController
    {
        public UnleashYourselfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowLocationOfCards(new LinqCardCriteria(c => c == LycanForm));
            SpecialStringMaker.ShowLocationOfCards(new LinqCardCriteria(c => c == Frenzy));
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                var search = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(c => c == LycanForm), true, false, false, shuffleAfterwards: true);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
                var fear = DealDamage(CharacterCard, IsVillainTarget, 1, DamageType.Psychic);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(fear); } else { this.GameController.ExhaustCoroutine(fear); }
            }
            if(ShouldActivate("wolf"))
            {
                List<SelectCardDecision> results = new List<SelectCardDecision>();
                
                var frenzy = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(c => c == Frenzy), true, false, false, false, storedResults: results, shuffleAfterwards: true);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(frenzy); } else { this.GameController.ExhaustCoroutine(frenzy); }
                if(!DidSelectCards(results))
                {
                    var rend = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 3, DamageType.Melee, 1, false, 0, false, false, false, c => !c.IsHero, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
                }
            }
        }
    }
}
