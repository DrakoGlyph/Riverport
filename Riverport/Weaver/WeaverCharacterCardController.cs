using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Weaver
{
    public class WeaverCharacterCardController : HeroCharacterCardController
    {
        public WeaverCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Weaver deals 1 Target 1 Projectile damage.
            //That target deals 1 less damage until the start of your turn
            
            var needle = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, CharacterCard), 1, DamageType.Projectile, 1, false, 0, false, true, false, addStatusEffect: Needlepoint, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(needle); } else { this.GameController.ExhaustCoroutine(needle); }
        }

        private IEnumerator Needlepoint(DealDamageAction arg)
        {
            // Reduce Damage dealt by the target by 1 until the start of your next turn
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.UntilStartOfNextTurn(TurnTaker);
            rdse.SourceCriteria.IsSpecificCard = arg.Target;

            var needle = AddStatusEffect(rdse);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(needle); } else { this.GameController.ExhaustCoroutine(needle); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        //destroy an Ongoing or Device
                        var destroy = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsOngoing || c.IsDevice), false, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
                        break;
                    }
                case 1:
                    {
                        //One player may play a card now
                        var guide = SelectHeroToPlayCard(HeroTurnTakerController);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(guide); } else { this.GameController.ExhaustCoroutine(guide); }
                        break;
                    }
                case 2:
                    {
                        //Reduce damage dealt to Hero characters 
                        ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
                        rdse.TargetCriteria.IsHero = true;
                        rdse.UntilStartOfNextTurn(TurnTaker);
                        var protect = AddStatusEffect(rdse);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(protect); } else { this.GameController.ExhaustCoroutine(protect); }
                        break;
                    }
            }
        }
    }
}
