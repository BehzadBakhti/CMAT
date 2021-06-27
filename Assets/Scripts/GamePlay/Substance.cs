using Creature;
using System.Collections.Generic;
using UnityEngine;

namespace Chemicals
{



    public class Substance
    {
        public SubstanceType type;
        public List<SubstanceElement> elements;
    }


    public enum SubstanceType
    {
        Selector,
        Effector,
        NegativeAgent,
        Compound,
    }

    public enum SubstanceEffect
    {
        Colonizer,
        Expander,
        Elongator,
        Hardner,
        Roughner,
    }

    public class SubstanceElement
    {
        public SubstanceEffect effect;
        public BodyPartEnum bodyPart;
        public int activity; // between -1 and 1
        public float dose;

        public float FinalEffect()
        {
            return activity * dose;
        }

        public bool IsSameElement(SubstanceElement other)
        {
            if (effect == other.effect && bodyPart == other.bodyPart)
            {
                return true;
            }

            return false;
        }
    }


    public class SubstanceMixer
    {
        private List<Substance> _inputs;
        private Substance _compound;

        public SubstanceMixer(List<Substance> inputs)
        {
            _inputs = inputs;
        }

        public Substance Mix()
        {
            _compound = new Substance();
            for (int i = 0; i < _inputs.Count; i++)
            {
                for (int j = 0; j < _inputs[i].elements.Count; j++)
                {

                    _compound.elements.Add(_inputs[i].elements[j]);
                }
            }

            for (int i = 0; i < _compound.elements.Count; i++)
            {
                for (int j = 0; j < _compound.elements.Count; j++)
                {
                    if (i != j)
                    {
                        if (_compound.elements[i].IsSameElement(_compound.elements[j]))
                        {
                            if (_compound.elements[i].dose > 0 && _compound.elements[j].dose > 0)
                            {
                                var eff = _compound.elements[i].FinalEffect() + _compound.elements[j].FinalEffect();

                                _compound.elements[i].dose = Mathf.Abs(eff);
                                _compound.elements[i].activity = (int)Mathf.Sign(eff);
                                _compound.elements[j].dose = 0;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < _compound.elements.Count; i++)
            {

                if (_compound.elements[i].dose <= 0)
                {
                    _compound.elements.RemoveAt(i);
                }

            }
            return _compound;
        }
    }


}
