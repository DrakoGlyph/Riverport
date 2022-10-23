using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirPaleMoonPendantCardController : BaseFenrirCardController
    {
        public FenrirPaleMoonPendantCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override void AddTriggers()
        {
            /*
             * Human: At the start of yor turn, you may draw a card.
             * Wolf: When you discard a card, Fenrir may regain 1 HP
             */

            AddStartOfTurnTrigger(tt => IsHuman && tt == TurnTaker, pca => DrawCard(HeroTurnTaker, true), TriggerType.DrawCard);
            AddTrigger<DiscardCardAction>(dca => IsWolf && dca.CardToDiscard.Owner == TurnTaker, dca => this.GameController.GainHP(CharacterCard, 1, cardSource: GetCardSource()), TriggerType.GainHP, TriggerTiming.After, isActionOptional: true);
        }
    }
}
