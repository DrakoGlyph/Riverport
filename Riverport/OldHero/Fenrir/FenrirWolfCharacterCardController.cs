using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class FenrirWolfCharacterCardController : FenrirBaseCharacterCardController
    {
        public FenrirWolfCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddStartOfTurnTrigger(tt => tt == TurnTaker, PayTheCost, TriggerType.DiscardCard);
        }

        public override bool CanBeMovedOutOfGame => true;


        private IEnumerator PayTheCost(PhaseChangeAction arg)
        {
            List<DiscardCardAction> result = new List<DiscardCardAction>();
            var discard = SelectAndDiscardCards(DecisionMaker, 1, false, 0, result, false, null, () => "Select a card to feed the wolf.");
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(discard); } else { this.GameController.ExhaustCoroutine(discard); }
            if (!DidDiscardCards(result))
            {
                var destroy = Detransform();
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }


        public override IEnumerator UsePower(int index = 0)
        {
            var destroy = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, Fenrir, 2, DamageType.Melee, 1, false, 0, false, false, false, c => !c.IsHero, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        var rend = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("device") || c.DoKeywordsContain("ongoing")), true, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
                        break;
                    }
                case 1:
                    {
                        IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                        idse.SourceCriteria.IsHero = true;
                        idse.UntilStartOfNextTurn(TurnTaker);
                        var inspire = AddStatusEffect(idse);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(inspire); } else { this.GameController.ExhaustCoroutine(inspire); }
                        break;
                    }
                case 2:
                    {
                        var draw = this.GameController.SelectHeroToDrawCard(HeroTurnTakerController, numberOfCards: 1, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                        break;
                    }
            }
        }
    }
}
