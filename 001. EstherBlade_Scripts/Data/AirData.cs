using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AirData
{
    [field: Header("JumpData")]
    [field: SerializeField][field: Range(0f, 25f)] public float JumpForce { get; private set; } = 4f;
    [field: SerializeField][field: Range(0f, 2f)] public float JumpSpeedModifier { get; private set; } = 0.8f;

    [field: Header("FallData")]
    [field: SerializeField][field: Range(0f, 2f)] public float FallSpeedModifier { get; private set; } = 0.5f;
}
