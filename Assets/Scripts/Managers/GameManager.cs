using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }
    [Header("Player")]
    public GameObject _player;
    [Header("Enemy")]
    public GameObject chestPrefab;
    public GameObject chestBossPrefab;
    public EnemySpawner enemySpawner;
    [Header("UI")]
    public Canvas _hud;
    public GameObject _chestUI;
    public GameObject _itemInfo;
    public Transform _chestContent;
    public GameObject _menuPause; 
    public GameObject _menuDeath;
    public GameObject _menuVictory;




    public TMP_Text waveText;

    public int waveNumber = 0;

    private int bossNumber;

    public bool gameWon = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        InventoryManager.Instance.ListItems();
        Application.runInBackground = true;
        CursorToggle(false);

    }

    public GameObject Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    public Canvas Hud
    {
        get
        {
            return _hud;
        }
        set
        {
            _hud = value;
        }
    }

    public GameObject ItemInfo
    {
        get
        {
            return _itemInfo;
        }
        set
        {
            _itemInfo = value;
        }
    }

    public GameObject ChestUI
    {
        get
        {
            return _chestUI;
        }
        set
        {
            _chestUI = value;
        }
    }

    public Transform ChestContent
    {
        get
        {
            return _chestContent;
        }
        set
        {
            _chestContent = value;
        }
    }

    public PlayerHealth PlayerHealth
    {
        get
        {
            return _player.GetComponent<PlayerHealth>();
        }
    }

    public bool GameWon
    {
        get
        {
            return gameWon;
        }
        set
        {
            gameWon = value;
        }
    }

    void Update() {

        if (enemySpawner.enemyCount == 0 && !enemySpawner.isSpawningWave) {

            if (waveNumber == 11) {

                VictoryMenu();

            }

            if (!gameWon) {

                if (waveNumber % 10 == 0 && waveNumber != 0) {
                
                    GameObject chest = Instantiate(chestBossPrefab, new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)), Quaternion.identity);
                
                } else {

                    GameObject chest = Instantiate(chestPrefab, new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)), Quaternion.identity);

                }

                waveNumber++;
                waveText.text = "Wave " + waveNumber;

                if (waveNumber % 10 == 0)
                {

                    bossNumber = (waveNumber / 10) % 5;

                    enemySpawner.bossPerWave = bossNumber;
                    enemySpawner.ResetEnemiesSpawnedForWave();
                    enemySpawner.StartCoroutine(enemySpawner.SpawnWave());

                }
                else
                {

                    enemySpawner.enemiesPerWave = waveNumber * 2;
                    enemySpawner.ResetEnemiesSpawnedForWave();
                    enemySpawner.StartCoroutine(enemySpawner.SpawnWave());

                }

            }

        }

    }

    public void CursorToggle(bool visible)
    {
        Cursor.visible = visible;
        Player.GetComponent<Inputs>().cursorInputForLook = !visible;

        if (visible)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }


    public void PauseMenu()
    {
        _menuPause.SetActive(true);
    }

    public void DeathMenu()
    {
        _menuDeath.SetActive(true);
    }

    public void VictoryMenu()
    {
        _menuVictory.SetActive(true);
    }
}
