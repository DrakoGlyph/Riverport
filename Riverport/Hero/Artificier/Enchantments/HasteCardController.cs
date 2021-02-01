using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class HasteCardController : EnchantmentBaseCardController
    {
        public HasteCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
        }

        protected override TriggerType TriggerType
        {
            get { return TriggerType.DrawCard; }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return GameController.ActiveTurnPhase.IsPlayCard && GameController.ActiveTurnTaker == Enchanted.Owner;
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddAdditionalPhaseActionTrigger(tt => tt == Enchanted.Owner, Phase.PlayCard, 1);
        }

        protected override bool DetermineEligible(Card c)
        {
            return c.IsHeroCharacterCard;
        }

        protected override IEnumerator WhenDestroyed(DestroyCardAction dca)
        {
            HeroTurnTaker htt = Enchanted.Owner as HeroTurnTaker;
            var draw = DrawCard(htt, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
        }

        public override IEnumerator Play()
        {
            var play = IncreasePhaseActionCountIfInPhase(tt => tt == Enchanted.Owner, Phase.PlayCard, 1);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
