using System;
using Creature;
using System.Collections.Generic;
using UnityEngine;

namespace Chemicals
{

    [Serializable]
    public class Substance
    {
        public SubstanceType type;
        public string nameTag;
        public float amount;
        public List<SubstanceElement> elements;
        public List<int> doses;
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
    [Serializable]
    public class SubstanceElement
    {
        public string nameTag;
        public SubstanceEffect effect;
        public BodyPartEnum bodyPart;
        public int activity; // between -1 and 1



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


        public Substance Mix(List<Substance> inputs)
        {
            _inputs = inputs;
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
                            if (_compound.doses[i]> 0 && _compound.doses[j]> 0)
                            {
                                var eff = _compound.elements[i].activity*_compound.doses[i] + _compound.elements[j].activity*_compound.doses[j];

                                _compound.doses[i] = Mathf.Abs(eff);
                                _compound.elements[i].activity = (int)Mathf.Sign(eff);
                                _compound.doses[j] = 0;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < _compound.elements.Count; i++)
            {

                if (_compound.doses[i] <= 0)
                {
                    _compound.elements.RemoveAt(i);
                    _compound.doses.RemoveAt(i);
                }

            }
            return _compound;
        }
    }
}
