using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int coins;
    [SerializeField] private int hp;
    [SerializeField] private int maxhp;
    [SerializeField] private Slider hpBar;

    private void Awake()
    {
        hpBar.maxValue = hp;
        hpBar.value = hp;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        hpBar.value = hp;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SellItem(int price)
    {
        coins += price;
    }
}
