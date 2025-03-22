using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Enemy : UnitBaseClass
{
    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    /*Effects Player Nexus Health
    public float GetDamageValue()
    {
        return damage;
    }*/
}
