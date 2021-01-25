using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public abstract class DragonCardController : CardController
    {
        protected DragonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsUnderCard(card);
        }
        

        protected IEnumerator DestroyCardUnderThis(PhaseChangeAction arg = null)
        {
            if (NumberOfCardsUnder > 1)
            {
                List<SelectCardDecision> chosen = new List<SelectCardDecision>();
                var decide = this.GameController.SelectCardAndStoreResults(HeroTurnTakerController, SelectionType.DestroyCard, Card.UnderLocation.Cards, chosen, false, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                var destroy = this.GameController.DestroyCard(HeroTurnTakerController, GetSelectedCard(chosen), cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            } else if(NumberOfCardsUnder == 1)
            {
                var dest = this.GameController.DestroyCard(HeroTurnTakerController, Card.UnderLocation.TopCard, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(dest); } else { this.GameController.ExhaustCoroutine(dest); }
            }
        }

        public int NumberOfCardsUnder
        {
            get
            {
                return Card.UnderLocation.NumberOfCards;
            }
        }

        public bool HasCardsUnder
        {
            get
            {
                return Card.UnderLocation.HasCards;
            }
        }
    }
}
