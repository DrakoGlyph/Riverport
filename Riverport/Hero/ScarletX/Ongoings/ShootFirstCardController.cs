using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class ShootFirstCardController : CardController
    {
        public ShootFirstCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowIfElseSpecialString(HasShotFirst, CanShootFirstString, ShotFirstString);
            
        }

        public string CanShootFirstString() { return "{ScarletX} can Shoot First"; }
        public string ShotFirstString() { return "{ScarletX} shot first"; }
        private bool HasShotFirst()
        {
            return IsPropertyTrue("ShotFirst");
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.DoKeywordsContain("arrow") && !HasBeenSetToTrueThisTurn("ShotFirst") && dca.ResponsibleCard != CharacterCard, ShootFirst, TriggerType.DestroyCard, TriggerTiming.Before); ;
            AddAfterLeavesPlayAction((GameAction ga) => ResetFlagAfterLeavesPlay("ShotFirst"), TriggerType.Hidden);
        }

        private IEnumerator ShootFirst(DestroyCardAction arg)
        {
            SetCardPropertyToTrueIfRealAction("ShotFirst");
            var destroy = this.GameController.DestroyCard(HeroTurnTakerController, arg.CardToDestroy.Card, true, responsibleCard: CharacterCard, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
        }
    }
}
