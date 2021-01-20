using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class FrenzyCardController : FenrirBaseCardController
    {
        public FrenzyCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, DestructionTriggers, TriggerType.DestroySelf);
            AddIncreaseDamageTrigger(dda => IsFenrir(dda.DamageSource.Card), dda => (int) Math.Floor(Card.UnderLocation.NumberOfCards / 2.0));
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => (dca.CardToDestroy.Card.IsTarget || dca.CardToDestroy.Card.IsDevice) && IsFenrir(dca.ResponsibleCard), DoFrenzy, TriggerType.Other, TriggerTiming.After);

        }

        private IEnumerator DoFrenzy(DestroyCardAction arg)
        {
            arg.SetPostDestroyDestination(Frenzy.UnderLocation);
            return DoNothing();
            //var consume = this.GameController.Change(TurnTakerController, arg.CardToDestroy.Card, Card.UnderLocation, cardSource: GetCardSource());
            //if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(consume); } else { this.GameController.ExhaustCoroutine(consume); }
        }

        private IEnumerator DestructionTriggers(PhaseChangeAction arg)
        {
            var poof = this.GameController.DestroyCard(HeroTurnTakerController, Card, cardSource: GetCardSource());

            if (ShouldActivate("human"))
            {
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(poof); } else { this.GameController.ExhaustCoroutine(poof); }
            }
            if(ShouldActivate("wolf"))
            {
                if(Card.UnderLocation.HasCards)
                {
                    var temper = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.Location == Card.UnderLocation), false);
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(temper); } else { this.GameController.ExhaustCoroutine(temper); }
                } else
                {
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(poof); } else { this.GameController.ExhaustCoroutine(poof); }
                }
            }
        }

        public override IEnumerator Play()
        {
            var destroy = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => !c.IsCharacter && (c.HitPoints <= 3 || c.IsDevice)), false, null, CharacterCard, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }
    }
}
