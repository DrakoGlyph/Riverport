using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public abstract class TreatCardController : CardController
    {
        public SelectionType EffectType { get; protected set; }

        public TreatCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected abstract IEnumerator Effect(Card c);

        public override IEnumerator Play()
        {
            if (FindCardsWhere(card => card.DoKeywordsContain("dragon") && card.IsInPlayAndHasGameText).Any())
            {
                List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
                var select = this.GameController.SelectCardsAndDoAction(HeroTurnTakerController, new LinqCardCriteria(card => card.DoKeywordsContain("dragon") && card.IsInPlayAndNotUnderCard), EffectType, Effect, 1, false, 1, storedResults, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
                Card c = GetSelectedCard(storedResults);
                var move = this.GameController.MoveCard(TurnTakerController, Card, c.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
            else
            {
                var revive = this.GameController.SelectCardsFromLocationAndMoveThem(DecisionMaker, TurnTaker.Trash, 0, 1, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), new List<MoveCardDestination>() { new MoveCardDestination(TurnTaker.PlayArea) }, true, false, selectionType: SelectionType.PutIntoPlay, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(revive); } else { this.GameController.ExhaustCoroutine(revive); }
            }
        }
    }
}
