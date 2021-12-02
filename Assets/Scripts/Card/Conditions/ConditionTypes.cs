using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTypes
{
    [System.Serializable]
    public class BuffCondition : Condition
    {
        [SerializeField]
        private BuffInfo buff;

        [SerializeField]
        private bool has = true;

        public override bool IsSatisfied(Character character, Character target)
            => character.Buffs.HasBuff(buff) == has;
    }

    [System.Serializable]
    public class ControlCondition : Condition
    {
        [Tooltip("����")]
        [SerializeField]
        private bool stuck = true;

        public override bool IsSatisfied(Character character, Character target)
        {
            return (character.ControlledType >= ControlType.Stuck) == stuck;
        }
    }

    [System.Serializable]
    public class RangeCondition : Condition
    {
        private enum RangeType { WithIn, OutOf }

        [SerializeField]
        private int range;

        [SerializeField]
        private RangeType rangeType;

        public override bool IsSatisfied(Character character, Character target)
        {
            int dist = Game.GetDistance(character.transform, target.transform);
            return rangeType switch
            {
                RangeType.WithIn => dist <= range,
                RangeType.OutOf => dist > range,
                _ => false
            };
        }
    }

    [System.Serializable]
    public class TargetStatCondition : Condition
    {
        private enum CompareType { GreaterOrEqual, LessThan }
        private enum StatType
        {
            Hp,
        }
        [SerializeField] private StatType stat;
        [SerializeField] private CompareType compare;
        [SerializeField] private float val;

        public override bool IsSatisfied(Character character, Character target)
        {
            switch (stat)
            {
                case StatType.Hp:
                    if (compare == CompareType.GreaterOrEqual)
                        return target.CurrentHealth >= val;
                    else return target.CurrentHealth < val;
                default:
                    break;
            }
            return false;
        }
    }
}
