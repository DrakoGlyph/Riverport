using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirEndurePainCardController : BaseFenrirCardController
    {
        public FenrirEndurePainCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, false)
        {
        }

        public override void AddTriggers()
        {
            /*
             * Reduce the damage dealt to Fenrir by Villain targets by 1
             * Increase Infernal damage dealt to Fenrir by 1
             * When Fenrir would take more than 3 damage in a single blow, you may destroy this card to negate that damage 
             */

            AddReduceDamageTrigger(dda => dda.Target == CharacterCard && dda.DamageSource.IsVillain, dda => 1);
            AddIncreaseDamageTrigger(dda => dda.Target == CharacterCard && dda.DamageType == DamageType.Infernal, 1);
            AddTrigger<DealDamageAction>(dda => dda.Target == CharacterCard && dda.Amount > 3, Endure, TriggerType.CancelAction, TriggerTiming.Before);
        }

        private IEnumerator Endure(DealDamageAction arg)
        {
            // ...you may destroy this card to negate that damage
            List<DestroyCardAction> destroyed = new List<DestroyCardAction>();
            var e = this.GameController.DestroyCard(HeroTurnTakerController, Card, true, destroyed, "Destroy this card to negate damage?", true, arg, Card, null, () => CancelAction(arg), cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }
    }
}
