using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class OverloadManaCardController : ArtificierBaseCardController
    {

        public OverloadManaCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            TrackMana();
        }


        public override IEnumerator Play()
        {
            int mana = ManaPool.CurrentValue;
            var pay = this.GameController.RemoveTokensFromPool(ManaPool, mana, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
            var damage = DealDamage(CharacterCard, c => c == CharacterCard || !c.IsHero, mana, DamageType.Energy);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(mana);
            rdse.SourceCriteria.IsSpecificCard = CharacterCard;
            rdse.UntilEndOfNextTurn(TurnTaker);
            var status = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }

            //Prevent Tokens added to Mana Pool until End of Next Turn

            //tokenPrevent = new Trigger<AddTokensToPoolAction>(GameController, attp => attp.TokenPool == ManaPool, attp => CancelAction(attp, false, true, null, true), new List<TriggerType>() { TriggerType.CancelAction }, TriggerTiming.Before, GetCardSource());
            //AddToTemporaryTriggerList(tokenPrevent);
            
            //OnPhaseChangeStatusEffect pcse = new OnPhaseChangeStatusEffect(Card, "RemoveTokenTrigger", "removed the restriction on Adding tokens to the Mana Pool", new TriggerType[1] { TriggerType.RemoveTrigger }, Card);
            
            //pcse.TurnPhaseCriteria.ExcludeRoundNumber = Game.TurnIndex;
            //pcse.TurnPhaseCriteria.Phase = Phase.End;
            //pcse.TurnPhaseCriteria.TurnTaker = TurnTaker;
            //pcse.UntilEndOfNextTurn(TurnTaker);
            //status = AddStatusEffect(pcse, false);
            //if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }
        }

    }
}
