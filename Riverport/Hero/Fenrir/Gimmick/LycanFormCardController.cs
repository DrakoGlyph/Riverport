using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class LycanFormCardController : FenrirBaseCardController
    {
        public LycanFormCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddBeforeDestroyAction(Detransform);
            AddStartOfTurnTrigger(tt => tt == TurnTaker, PayTheCost, TriggerType.DealDamage);
            AddIncreaseDamageTrigger(dda => !dda.Target.IsHero && IsFenrir(dda.DamageSource.Card), 1);
        }

        private IEnumerator PayTheCost(PhaseChangeAction arg)
        {
            List<DiscardCardAction> result = new List<DiscardCardAction>();
            var discard = SelectAndDiscardCards(DecisionMaker, 1, false, 0, result, false, null, () => "Select a card to feed the wolf.");
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(discard); } else { this.GameController.ExhaustCoroutine(discard); }
            if (!DidDiscardCards(result))
            {
                var destroy = this.GameController.DestroyCard(DecisionMaker, LycanForm, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }

        private IEnumerator Detransform(GameAction arg)
        {
            if (!Human.IsInPlay)
            {
                var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Human, true, false, false, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
            }
            
        }

        public override IEnumerator Play()
        {
            //Switch To Wolf Form
            if(!Wolf.IsInPlay)
            {
                var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Wolf, true, false, false, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var destroy = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, Fenrir, 2, DamageType.Melee, 1, false, 0, false, false, false, c=>!c.IsHero, cardSource:GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }
    }
}
