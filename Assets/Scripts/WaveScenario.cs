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

    public Transform spawnPoint;

    enum WaveScenarioState
    {
        Pause,
        Wave
    }

    WaveScenarioState state;

    void Start()
    {
        nextWaveTime = pauseTime * 2;
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

                    int meleeNumber = (int)Random.Range(0, waveUnitsNumber);

                    for (int i = 0; i < meleeNumber; i++)
                    {
                        GameEntity entity = Instantiate(meleeEnemy,spawnPoint.position, spawnPoint.rotation).GetComponent<GameEntity>();
                        entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
                        currentWaveEnemies.Add(entity);
                    }
                    for (int i = meleeNumber; i < waveUnitsNumber; i++)
                    {
                        GameEntity entity = Instantiate(missileEnemy, spawnPoint.position, spawnPoint.rotation).GetComponent<GameEntity>();
                        entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
                        currentWaveEnemies.Add(entity);
                    }

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
}
