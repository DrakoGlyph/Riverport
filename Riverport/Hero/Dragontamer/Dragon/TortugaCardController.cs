﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class TortugaCardController : DragonCardController
    {
        public TortugaCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            //When a dragon would be dealt damage, you may destroy a card under this one to prevent that damage
            AddTrigger<DealDamageAction>((DealDamageAction dda) => HasCardsUnder && (dda.Target.DoKeywordsContain("dragon") || dda.Target == CharacterCard), Protect, TriggerType.WouldBeDealtDamage, TriggerTiming.Before, orderMatters: true, priority: TriggerPriority.High);
        }

        private IEnumerator Protect(DealDamageAction arg)
        {
            if (arg.Amount > 0)
            {
                List<YesNoCardDecision> doIt = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.PreventDamage, Card, arg, doIt, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(doIt))
                {
                    var pay = DestroyCardUnderThis();
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                    var protect = CancelAction(arg);
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(protect); } else { this.GameController.ExhaustCoroutine(protect); }
                }
            }
        }

        public override IEnumerator Play()
        {
            //When Tortuga enters play, you may put up to 3 cards from your hand under him
            List<MoveCardDestination> under = new List<MoveCardDestination>() { new MoveCardDestination(Card.UnderLocation) };
            List<MoveCardAction> result = new List<MoveCardAction>();
            var empower = this.GameController.SelectCardsFromLocationAndMoveThem(HeroTurnTakerController, HeroTurnTaker.Hand, 0, 3, new LinqCardCriteria(c => true), under, storedResultsMove: result, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(empower); } else { this.GameController.ExhaustCoroutine(empower); }
            int count = GetNumberOfCardsMoved(result);
            if(count > 0)
            {
                var draw = DrawCards(HeroTurnTakerController, count);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Put the top card of your deck under Tortuga
            var move = this.GameController.MoveCard(TurnTakerController, TurnTaker.Deck.TopCard, Card.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
        }
    }
}
