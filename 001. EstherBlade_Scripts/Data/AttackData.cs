using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackType
{
    Base,
    Combo
}

public enum DetectionType
{
    WeaponCollider,
    BoxCast
}

[Serializable]
public class AttackInfoData
{
    [field: Header("General Setting")]
    [field: SerializeField] public AttackType AttackType { get; private set; }
    [field: SerializeField] public string AttackName { get; private set; }
    public int AttackNameHash { get; private set; }

    [field: SerializeField] public int Damage { get; private set; }

    [field: Header("Force Settings")]
    [field: SerializeField][field: Range(0f, 1f)] public float ForceTransitionTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }

    [field: Header("Timing Settings")]
    [field: SerializeField][field: Range(0f, 1f)] public float ActivationTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }

    [field: Header("Detection Type Settings")]
    [field: SerializeField] public DetectionType DetectionType { get; private set; }
    [field: SerializeField] public Vector3 BoxCastSize { get; private set; }

    public void GenerateHash()
    {
        AttackNameHash = Animator.StringToHash(AttackName);
    }

}


[Serializable]
public class AttackData
{
    [field: SerializeField] public AttackInfoData BaseAttackInfo { get; private set; }
    [field: SerializeField] public List<int> SkillIndexSlot { get; private set; }
    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; }

    public AttackInfoData GetSkillInfo(int slotIndex)
    {
        if (SkillIndexSlot.Count <= slotIndex) return null;

        int skillID = SkillIndexSlot[slotIndex];
        return AttackInfoDatas.Count <= skillID ? null : AttackInfoDatas[skillID];
    }

    public AttackInfoData GetAttackInfo(int index)
    {
        return AttackInfoDatas.Count <= index ? null : AttackInfoDatas[index];
    }

    public void Initialize()
    {
        BaseAttackInfo.GenerateHash();
        foreach (var attackInfo in AttackInfoDatas)
        {
            attackInfo.GenerateHash();
        }
    }
}
