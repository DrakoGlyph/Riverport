using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class OtherHandCardController : FenrirBaseCardController
    {
        public OtherHandCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialString ss = SpecialStringMaker.ShowHasBeenUsedThisTurn("OtherHand", "Fenrir has dealt Other Hand damage this turn.", "Fenrir can deal Other hand damage this turn");
            ss.Condition = ShouldActivateWolf;
        }

        public override void AddTriggers()
        {
            //AddStartOfTurnTrigger(tt => true, Reset, TriggerType.Other);
            AddTrigger<PlayCardAction>((PlayCardAction pca) => pca.IsSuccessful && !pca.CardToPlay.DoKeywordsContain("feral") && IsFirstTimeCardPlayedThisTurn(pca.CardToPlay, c => !c.DoKeywordsContain("feral"), TriggerTiming.After), OtherHumanHand, TriggerType.PlayCard, TriggerTiming.After);
            AddTrigger<DealDamageAction>((DealDamageAction dda) => !IsPropertyTrue("OtherHand") && dda.DidDealDamage && dda.DamageType == DamageType.Melee && IsFenrir(dda.DamageSource.Card), OtherWolfHand, TriggerType.DealDamage, TriggerTiming.After);
            AddAfterLeavesPlayAction((GameAction ga) => ResetFlagAfterLeavesPlay("OtherHand"), TriggerType.Hidden);
        }

        private IEnumerator OtherWolfHand(DealDamageAction arg)
        {
            if (ShouldActivate("wolf"))
            {
                SetCardPropertyToTrueIfRealAction("OtherHand");
                var fear = DealDamage(CharacterCard, arg.Target, 1, DamageType.Psychic, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(fear); } else { this.GameController.ExhaustCoroutine(fear); }

            }
        }

        private IEnumerator OtherHumanHand(PlayCardAction arg)
        {
            if(ShouldActivate("human"))
            {
                var play = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => !c.DoKeywordsContain("feral")));
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
            }

        }

        private IEnumerator Reset(PhaseChangeAction arg)
        {
            SetCardProperty("OtherHand", false);
            return DoNothing();
        }


    }
}
