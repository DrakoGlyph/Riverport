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
            SpecialStringMaker.ShowLocationOfCards(new LinqCardCriteria(c => c == Frenzy));
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                //var search = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(c => c == LycanForm), true, false, false, shuffleAfterwards: true);
                var transform = Transform();
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(transform); } else { this.GameController.ExhaustCoroutine(transform); }
                List<YesNoCardDecision> doHowl = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.UsePowerOnCard, CharacterCard, null, doHowl, null, GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(doHowl))
                {
                    var howl = UsePowerOnOtherCard(CharacterCard, 0);
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(howl); } else { this.GameController.ExhaustCoroutine(howl); }
                }
            }
            if (ShouldActivate("wolf"))
            {

                List<SelectCardDecision> result = new List<SelectCardDecision>();
                var frenzy = SearchForCards(DecisionMaker, true, true, 0, 1, new LinqCardCriteria(c => c.Identifier == "Frenzy"), false, true, false, true, result, false, true);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(frenzy); } else { this.GameController.ExhaustCoroutine(frenzy); }

                List<Function> functionChoices = new List<Function>()
                {
                    new Function(DecisionMaker, "Play a card", SelectionType.PlayCard, () => SelectAndPlayCardFromHand(DecisionMaker, associateCardSource: true), CanPlayCards(TurnTakerController), "Fenrir could only play a card!"),
                    new Function(DecisionMaker, "Deal 1 target 3 Melee damage", SelectionType.DealDamage, () => this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 3, DamageType.Melee, 1, false, 0, false, false, false, c => !c.IsHero, cardSource: GetCardSource()), AskIfCardCanDealDamage(CharacterCard), "Fenrir was forced to deal damage!")
                };
                var rend = SelectAndPerformFunction(DecisionMaker, functionChoices, true);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }

            }
        }
    }
}
