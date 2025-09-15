using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Characters/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public GroundData GroundData { get; private set; }
    [field: SerializeField] public AirData AirData { get; private set; }
    [field: SerializeField] public AttackData AttackData { get; private set; }

    [field: SerializeField] public float SearchingDistance { get; private set; }
}