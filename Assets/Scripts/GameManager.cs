using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject mainCam;
    public CharCtrl character;
    public GameObject enemies;
    public float playTime;
    public bool isBattle;
    public int enemycount;

    public GameObject menuPanel;
    public GameObject gamePanel;

    public Text scoreText;
    public Text playtimeText;
    public Text playerHealthText;
    public Text playerAmmoText;
    public Text playerCoinText;

    public void GameStart()
    {
        menuCam.SetActive(false);
        mainCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        character.gameObject.SetActive(true);
        enemies.SetActive(true);
    }

    private void LateUpdate()
    {
        scoreText.text = string.Format("{0:n0}", character.score);
        playerHealthText.text = character.heart.ToString();

        playerCoinText.text = string.Format("{0:n0}", character.coin);

        playerAmmoText.text = character.ammo.ToString();
    }
}
