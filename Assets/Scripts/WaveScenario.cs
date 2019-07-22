using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//has its own UI element for now, maybe later we add it to the overall UI Manager, - shows the wave number - how many enemies are left and the pause time
public class WaveScenario : MonoBehaviour
{
    public float waveUnitsNumber;
    public float waveRiser; //the units number gets bigger by this amount every wave
    public float pauseTime;
    public float prepTime;

    float nextWaveTime;
    int waveNumber = 0;

    HashSet<GameEntity> currentWaveEnemies = new HashSet<GameEntity>();

    public GameObject meleeEnemy;
    public GameObject missileEnemy;

    public GameObject pausePanel;
    public Text pauseTimeLeft;
    public GameObject wavePanel;
    public Text waveEnemiesLeft;
    public Text waveNumberUI;

    public Transform[] spawnPoints;

    enum WaveScenarioState
    {
        Pause,
        Wave
    }

    WaveScenarioState state;

    void Start()
    {
        nextWaveTime = prepTime;
    }

    void Update()
    {
        switch (state)
        {
            case WaveScenarioState.Pause:

                if (Time.time > nextWaveTime)
                {
                    state = WaveScenarioState.Wave;
                    //spawn enemies:
                    waveNumber++;


                    SpawnUnits();

                    waveUnitsNumber += waveRiser;


                    pausePanel.SetActive(false);
                    wavePanel.SetActive(true);
                }
                else
                {
                    pauseTimeLeft.text = (nextWaveTime - Time.time).ToString();
                }

                break;

            case WaveScenarioState.Wave:

                if (currentWaveEnemies.Count == 0)
                {
                    state = WaveScenarioState.Pause;
                    nextWaveTime = Time.time + pauseTime;

                    pausePanel.SetActive(true);
                    wavePanel.SetActive(false);

                }
                else
                {
                      waveEnemiesLeft.text = currentWaveEnemies.Count.ToString();
                      waveNumberUI.text = waveNumber.ToString();
                }
                break;
        }
    }

    public void OnEnemyFromThisWaveDies(GameEntity entity)
    {
        currentWaveEnemies.Remove(entity);
    }

    //Refactor this, not ideal
    void SpawnUnits()
    {
        //we randomly dispose the units around the spawnpoints
        int unitsLeft = (int)waveUnitsNumber;
        float approxNumberPerPoint =  (unitsLeft * 1f / spawnPoints.Length);

        while (unitsLeft>0)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Random.seed = System.DateTime.Now.Millisecond;
                int unitsAtThisPoint = (int)Random.Range(approxNumberPerPoint / 2, approxNumberPerPoint * 2);

                if (unitsAtThisPoint > unitsLeft) unitsAtThisPoint = unitsLeft;
                int meleesAtThisPoint = (int)Random.Range(0, unitsAtThisPoint);

                for (int j = 0; j < meleesAtThisPoint; j++)
                {
                    GameEntity entity = Instantiate(meleeEnemy, spawnPoints[i].position, spawnPoints[i].rotation).GetComponent<GameEntity>();
                    entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
                    currentWaveEnemies.Add(entity);
                    unitsLeft--;

                }
                for (int j = meleesAtThisPoint; j < unitsAtThisPoint; j++)
                {
                    GameEntity entity = Instantiate(missileEnemy, spawnPoints[i].position, spawnPoints[i].rotation).GetComponent<GameEntity>();
                    entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
                    currentWaveEnemies.Add(entity);
                    unitsLeft--;


                }



            }
        }

        


        
    }
}
