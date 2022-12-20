using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ProjectileAttacks",menuName ="ProjectileAttacks")]
public class ProjectileAttacks :ScriptableObject
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private LayerMask whatIsHitable;
}
