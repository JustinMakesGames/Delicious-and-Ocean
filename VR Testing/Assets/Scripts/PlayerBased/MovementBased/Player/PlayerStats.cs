using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : ActorBase
{
    public static Transform PlayerPos { get; private set; }
    public static PlayerStats Instance { get; private set; }
    public int coins;
    [SerializeField] private TMP_Text coinText;

    [SerializeField] private Slider hpBar;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        PlayerPos = transform;
        hpBar.maxValue = _currentHealth;
        hpBar.value = _currentHealth;
        coinText.text = coins.ToString();
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
        coinText.text = coins.ToString();
    }

    protected override void OnActorDeath()
    {
        WinManager.Instance.HandleLosing(); 
        //Enable death screen
    }

}
