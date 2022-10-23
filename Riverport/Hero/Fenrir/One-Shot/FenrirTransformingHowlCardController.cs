using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirTransformingHowlCardController : BaseFenrirCardController
    {
        public FenrirTransformingHowlCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
            
        }

        public override IEnumerator Play()
        {
            /*
             * Transform.
             * Deal each Non-Hero target 1 Psychic damage.
             */
            IEnumerator e;
            // Transform is specifically into Wolf Form.
            if (!IsWolf)
            {
                e = Transform();
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            e = DealDamage(CharacterCard, c => c.IsTarget && !c.IsHero, 1, DamageType.Psychic);
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }
    }
}
