using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class BaseFenrirCardController : CardController
    {
        public DamageSource Fenrir
        {
            get
            {
                return new DamageSource(GameController, CharacterCard);
            }
        }

        public BaseFenrirCharacterCardController FenrirCharacterController
        {
            get
            {
                return this.GameController.FindCardController(CharacterCard) as BaseFenrirCharacterCardController; 
            }
        }

        public bool IsHuman => FenrirCharacterController.IsHuman;
        public bool IsWolf => FenrirCharacterController.IsWolf;
        public bool DoesFormMatter { get; }
        public IEnumerator Transform() => FenrirCharacterController.Transform();
        public IEnumerator Detransform() => FenrirCharacterController.Detransform();


        
        protected BaseFenrirCardController(Card card, TurnTakerController turnTakerController, bool doesFormMatter) : base(card, turnTakerController)
        {
            DoesFormMatter = doesFormMatter;
        }
    }
}
