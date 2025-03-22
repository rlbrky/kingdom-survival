using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nexus_Player : UnitBaseClass
{
    protected override void Awake()
    {
        _collider = GetComponent<Collider>();
        _currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = _currentHealth;
    }
    
    protected override IEnumerator DeathCoroutine()
    {
        //TODO: Play some VFX or smt
        Destroy(_collider);
        yield return new WaitForSeconds(1f);
        //TODO: ENDGAME
    }

    public override bool GetHit(float incDamage)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= incDamage;
            UpdateHealth(_currentHealth);
            _activeState = State.Damaged;
            return true;
        }
        else
        {
            _deathRoutine = StartCoroutine(DeathCoroutine());   
            return false;
        }
    }

    /*
     private void OnTriggerEnter(Collider other)
    {
        //TODO: Play Some VFX
        //NexusGotDamaged(other.GetComponent<Unit_Enemy>().GetDamageValue());
        //Destroy(other.gameObject);
    }
    private void NexusGotDamaged(float damageVal)
    {
        _currentHealth -= damageVal;
        healthSlider.value = _currentHealth;
        if (_currentHealth <= 0)
        {
            //TODO: End Game
        }
    }*/
}