using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    [Header("User Interface")]
    public Button nextWavebutton;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI moneyText;
    public GameObject combatUI;
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Player data")]
    public int lives;
    public float money;

    [Header("Creeps")]
    public GameObject creepPrefab;
    public Vector3 creepSpawn;
    public Vector3 creepTarget;
    public WaveSO currentWave;      //The wave we are currently spawning or about to spawn
    public int creepInWave;         //Which creep in this wave are we up to?
    public List<WaveSO> allWaves;   //All of the waves on this map
    public int waveInAll;           //Which wave on this map are we up to?
    int livingCreeps = 0;

    [Header("Towers")]
    public GameObject towerPrefab;
    [Tooltip("The one we have currently selected")]
    public TowerSO towerData;
    [Tooltip("All of the possible towers")]
    public List<TowerSO> allTowers;



    // Start is called before the first frame update
    void Start()
    {
        ChangeMoney();
        ChangeLives(0);
        combatUI.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectTower(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectTower(1);
        }
    }

    public bool BuySomething(float price)
    {
        if(price <= money)          //Is the price less than the reserved money?
        {
            money = money - price;  //If it is, then subtract the money
            ChangeMoney();
            return true;            //Return true
        }
        else
        {
            return false;           //Otherwise, just return false
        }
    }

    public void SelectTower(int value)
    {
        if(value < 0 || value >= allTowers.Count)
        {
            return;
        }
        towerData = allTowers[value];
        towerPrefab = towerData.prefab;
    }

    public void CreepDied(float creepValue)
    {
        money = money + creepValue;     //Add money
        ChangeMoney();
        livingCreeps--;                 //Reduce living creeps
        if(livingCreeps == 0)                       //If that was the last creep alive
        {
            if (creepInWave == 0)                   //And we have none left to spawn this wave
            {
                nextWavebutton.interactable = true;

                if (waveInAll >= allWaves.Count)    //And it was the last wave on this map
                {
                    //Finally we have killed them all
                    Debug.Log("You have beaten all the waves. Developer please put in a win screen here");
                    winScreen.SetActive(true);
                    combatUI.SetActive(false);
                }
            }
        }
    }

    public void SpawnNextCreep()
    {
        //Set the correct wave up
        currentWave = allWaves[waveInAll];

        //Make the next wave button non-interactable
        nextWavebutton.interactable = false;

        //Add one to the living creep counter.
        livingCreeps++;

        //Spawn in the new creep and get a reference to it
        GameObject newObject = Instantiate(creepPrefab);

        //Set the new objects position to spawnpoint
        newObject.transform.position = creepSpawn;

        //Get the component on it
        Creep creep = newObject.GetComponent<Creep>();

        //Tell creep where to go
        creep.objective = creepTarget;

        //Set all the important values
        creep.maxHealth = currentWave.creeps[creepInWave].maxHealth;
        creep.speed = currentWave.creeps[creepInWave].speed;
        creep.armour = currentWave.creeps[creepInWave].armour;
        creep.money = currentWave.creeps[creepInWave].money;

        //If we have not reached the last one in the list...
        if(creepInWave < currentWave.creeps.Count - 1)
        {
            //Add one to the counter
            creepInWave = creepInWave + 1;

            //Call this function again with a delay according to the wave.
            Invoke("SpawnNextCreep", currentWave.spacing[creepInWave]);
        }
        else
        {
            //If we have reached the end, then reset the counter and do not call this again until the player manually starts the next wave
            creepInWave = 0;

            //Also go to the next wave
            waveInAll++;
        }
    }

    void ChangeMoney()
    {
        moneyText.text = "Money: " + money;
    }

    public void ChangeLives(int value)
    {
        lives = lives + value;

        //If lives goes below 0 then you lose
        if (lives <= 0)
        {
            loseScreen.SetActive(true);
            combatUI.SetActive(false);
        }

        livesText.text = "Lives: " + lives;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(creepSpawn, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(creepTarget, 0.5f);
    }
}
