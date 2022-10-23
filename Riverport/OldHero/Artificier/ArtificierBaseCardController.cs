using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public abstract class ArtificierBaseCardController : CardController
    {
        public ArtificierBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            
        }

        protected void TrackMana()
        {
            SpecialStringMaker.ShowTokenPool(ManaPool);
        }

        protected bool IsEnchanted(Card c)
        {
            if(c.NextToLocation.HasCards)
            {
                return c.NextToLocation.Cards.Any(card => card.DoKeywordsContain("enchantment"));
            }
            return false;
        }

        public DamageSource Artificier
        {
            get
            {
                return new DamageSource(GameController, CharacterCard);
            }
        }

        private TokenPool _manaPool;

        public TokenPool ManaPool
        {
            get { 
                if(_manaPool==null)
                {
                    _manaPool = this.TurnTaker.FindCard("ArchiveGauntlets", false).FindTokenPool("ManaPool");
                }
                return _manaPool;
            }
        }


    }
}
