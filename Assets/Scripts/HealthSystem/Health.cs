using System.Diagnostics;
using UnityEngine;

public class Health
{
    private float _currHealth;
    private float _maxHealth;

    public float CurrentHealth { get { return _currHealth; }
        set
        {
            _currHealth = value;
            _currHealth = Mathf.Clamp(_currHealth,0,_maxHealth);
        }
            
            
    }

    public float MaxHealth { get { return _currHealth; } set { _maxHealth = value; } }


    public Health(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currHealth = _maxHealth;
    }

    public Health(float currHealth,float maxHealth)
    {
        _currHealth = currHealth;
        _maxHealth = maxHealth;
    }
}
