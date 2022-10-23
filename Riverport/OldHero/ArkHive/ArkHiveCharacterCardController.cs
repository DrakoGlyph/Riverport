using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections.Generic;
using System.Collections;

namespace Riverport.ArkHive
{
    public class ArkHiveCharacterCardController : ArkHiveBaseCharacterCardController
    {
        public ArkHiveCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            Location peekAt;
            List<SelectLocationDecision> result = new List<SelectLocationDecision>();
            var deck = this.GameController.SelectADeck(DecisionMaker, SelectionType.RevealTopCardOfDeck, l => l.IsVillain, result);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(deck); } else { this.GameController.ExhaustCoroutine(deck); }
            peekAt = GetSelectedLocation(result);

            var reveal = RevealCardsFromTopOfDeck_PutOnTopAndOnBottom(HeroTurnTakerController, TurnTakerController, peekAt, 1, 1, 0);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            List<Function> functions = new List<Function>()
            {
                new Function(DecisionMaker, "Play a Plan", SelectionType.PlayCard, () => SelectAndPlayCardFromHand(DecisionMaker, true, null, new LinqCardCriteria(c=>c.DoKeywordsContain("plan")), false, true)),
                new Function(DecisionMaker, "Enact a Plan", SelectionType.ActivateAbility, () => this.GameController.SelectAndActivateAbility(DecisionMaker, "enact", new LinqCardCriteria(c=>c.DoKeywordsContain("plan")), null, false, GetCardSource()))
            };
            var doIt = SelectAndPerformFunction(DecisionMaker, functions, true, null, null, "Unplanned Contingency prevented Ark-Hive from doing anything");
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(doIt); } else { this.GameController.ExhaustCoroutine(doIt); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        var pow = this.GameController.SelectHeroToUsePower(DecisionMaker, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pow); } else { this.GameController.ExhaustCoroutine(pow); }
                        break;
                    }
                case 1:
                    {
                        List<TurnTakerController> storedResults = null;
                        var villain = this.GameController.FindVillainTurnTakerController(DecisionMaker, SelectionType.DiscardFromDeck, storedResults, tt => tt.Deck.HasCards, GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(villain); } else { this.GameController.ExhaustCoroutine(villain); }
                        IEnumerator discard;
                        if (GetSelectedTurnTakerController(storedResults) != null)
                        {
                            discard = DiscardCardsFromTopOfDeck(GetSelectedTurnTakerController(storedResults), 1, false, null, true, TurnTaker);
                        } else
                        {
                            discard = this.GameController.SendMessageAction("No Villain was selected!", Priority.Medium, GetCardSource(), null, true);
                        }
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(discard); } else { this.GameController.ExhaustCoroutine(discard); }

                        break;
                    }
                case 2:
                    {
                        var play = SelectHeroToPlayCard(DecisionMaker);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
                        break;
                    }
            }
        }
    }
}
