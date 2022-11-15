using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{

    public int enemies = 5;
    public Text enemiesText;

    private void Awake()
    {
        enemiesText.text = enemies.ToString();

        Enemy.OnEnemyKilled += OnEnemyKilledActoin;
    }

    void OnEnemyKilledActoin()
    {
        enemies--;
        enemiesText.text = enemies.ToString();
    }
}
