﻿using Chemicals;
using MonstersDataManagement;
using UnityEngine;

namespace Creature
{
    public class BodyPart : MonoBehaviour
    {
        public BodyPartEnum BodyPartName;
        [SerializeField] private float _length;
        [SerializeField] private float _hardness;
        [SerializeField] private float _roughness;
        [SerializeField] private Creature _creature;
        [SerializeField] private Bone _partRootBone;

        public Creature Creature { get => _creature; set => _creature = value; }
        private void Start()
        {
            _partRootBone = GetComponent<Bone>();
        }
        public virtual void SetLength(float length)
        {
            _length = length;
        }

        public virtual void SetHardness(float hardness)
        {
            _hardness = hardness;
        }

        public virtual void ApplySubstance(SubstanceElement substance, float age, int mutationCount)
        {
            //ToDo: the effect of the substance is affected by the "substance.dose", "age"m and "MutationCount"
            Debug.Log(substance.bodyPart.ToString() + substance.effect.ToString() + substance.activity);
            switch (substance.effect)
            {
                case SubstanceEffect.Colonizer:
                    _creature.Colonize(this, substance.activity);
                    break;
                case SubstanceEffect.Expander:
                    Expand(substance.activity);
                    break;
                case SubstanceEffect.Elongator:
                    Elongate(substance.activity);
                    break;
                case SubstanceEffect.Hardner:
                    break;
                case SubstanceEffect.Roughner:
                    break;
                default:
                    break;
            }
        }

        private void Expand(int activity)
        {

        }

        private void Elongate(int activity)
        {
           _creature.Skeletone.Elongate(_partRootBone, activity);
        }

        private void Harden(int activity)
        {

        }

        private void Roughen(int activity)
        {

        }
    }
    public class Hand : BodyPart
    {

    }

    public enum BodyPartEnum
    {
        Head,
        Eye,
        Ear,
        Nose,
        Mouse,
        Tooth,
        Tongue,
        Neck,
        Spine,
        Foot,
        Wing,
        Tail
    }

}
