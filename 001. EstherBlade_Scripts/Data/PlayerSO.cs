using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public GroundData GroundData { get; private set; }
    [field: SerializeField] public AirData AirData { get; private set; }
    [field: SerializeField] public AttackData AttackData { get; private set; }
}
