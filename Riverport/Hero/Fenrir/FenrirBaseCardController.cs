using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public abstract class FenrirBaseCardController : CardController
    {
        protected FenrirBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowSpecialString(ShowForm);
        }

        protected string ShowForm()
        {
            string rtn = "{Fenrir} is in ";
            if (Human.IsInPlay) rtn += "{human}";
            if (Wolf.IsInPlay) rtn += "{wolf}";
            return rtn += " form.";
        }


        protected IEnumerator Detransform(GameAction arg = null)
        {
            if (!Human.IsInPlay)
            {
                var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Human, true, false, false, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
            }

        }

        protected IEnumerator Transform()
        {
            //Switch To Wolf Form
            if (!Wolf.IsInPlay)
            {
                var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Wolf, true, false, false, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
            }
        }


        protected Card Frenzy
        {
            get
            {
                return FindCard("Frenzy");
            }
        }

        protected DamageSource Fenrir
        {
            get
            {
                return new DamageSource(GameController, CharacterCard);
            }
        }
        
        protected bool ShouldActivate(string key)
        {
            switch(key)
            {
                case "human":
                    {
                        return Human.IsInPlay;
                    }
                case "wolf":
                    {
                        return Wolf.IsInPlay;
                    }
            }
            return false;
        }

        protected bool ShouldActivateWolf() { return ShouldActivate("wolf"); }
        
        protected Card Human
        {
            get
            {
                return FindCard("FenrirHumanCharacter");
            }
        }

        protected Card Wolf
        {
            get
            {
                return FindCard("FenrirWolfCharacter");
            }
        }

        protected bool IsFenrir(Card c)
        {
            return c == Human || c == Wolf;
        }
    }
}
