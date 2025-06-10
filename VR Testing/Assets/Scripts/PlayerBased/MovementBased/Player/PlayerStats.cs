using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : ActorBase
{
    public static Transform PlayerPos { get; private set; }
    public static PlayerStats Instance { get; private set; }
    public int coins;

    [SerializeField] private Slider hpBar;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        PlayerPos = transform;
        hpBar.maxValue = _currentHealth;
        hpBar.value = _currentHealth;
    }

    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        base.OnDamageTaken(damage, dmgType);
        hpBar.value = _currentHealth;
        print(_currentHealth + " HP left");
    }

    public void SellItem(int price)
    {
        coins += price;
    }

    protected override void OnActorDeath()
    {
       //Enable death screen
    }

}
