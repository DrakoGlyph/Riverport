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

        public override void AddTriggers()
        {
            
        }


        protected IEnumerator DestroyCardUnderThis(PhaseChangeAction arg = null)
        {
            List<SelectCardDecision> chosen = new List<SelectCardDecision>();
            var decide = this.GameController.SelectCardAndStoreResults(HeroTurnTakerController, SelectionType.DestroyCard, Card.UnderLocation.Cards, chosen, false, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            var destroy = this.GameController.DestroyCard(HeroTurnTakerController, GetSelectedCard(chosen), cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
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
