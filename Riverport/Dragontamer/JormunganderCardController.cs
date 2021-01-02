using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class JormunganderCardController : DragonCardController
    {
        public JormunganderCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            // Destroy an environment card and put it under Jormungander
            List<DestroyCardAction> destroyed = new List<DestroyCardAction>();
            var destroy = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsEnvironment), true, destroyed, Card, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            var move = this.GameController.MoveCard(TurnTakerController, GetDestroyedCards(destroyed).FirstOrDefault(), Card.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
        }


        private List<DestroyCardAction> destroyed = new List<DestroyCardAction>();

        public override IEnumerator UsePower(int index = 0)
        {
            //Destroy a card under this one, if you do, Destroy an environment card. 
            //You can put it under Jormungander or deal 2 Fire damage to a Villain Turn
            
            var destroy = DestroyCardUnderThis();
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }

            var raze = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsEnvironment), true, destroyed, Card, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(raze); } else { this.GameController.ExhaustCoroutine(raze); }


            Function move = new Function(HeroTurnTakerController, "Move card under Jormungander", SelectionType.MoveCard, Move);
            Function damage = new Function(HeroTurnTakerController, "Deal 2 Fire damage to 1 target", SelectionType.DealDamage, Damage);
            var select = SelectAndPerformFunction(HeroTurnTakerController, new List<Function>() { move, damage }, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }

            
        }

        private IEnumerator Move()
        {
            //destroyed should be updated
            Card feed = GetDestroyedCards(destroyed).FirstOrDefault();
            var move = this.GameController.MoveCard(TurnTakerController, feed, Card.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
        }

        private IEnumerator Damage()
        {
            var a = GetPowerNumeral(0, 1);
            var b = GetPowerNumeral(1, 2);
            var damage = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), b, DamageType.Fire, a, false, 0, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
        }
    }
}
