using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject AITank;

    public Button spawnAITankButton;
    public Button resetPlayerButton;

    public float spawnRange;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spawnAITankButton.onClick.AddListener(delegate { SpawnEnemyTank(); });
        resetPlayerButton.onClick.AddListener(delegate { ResetPlayer(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnEnemyTank()
    {
        Vector3 _spawnPoint = new Vector3(Random.Range(spawnRange / 2 , spawnRange), 0.1f, Random.Range(spawnRange / 2, spawnRange)) + new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Instantiate(AITank, _spawnPoint, Quaternion.identity);
    }

    private void ResetPlayer()
    {
        SceneManager.LoadScene(0);
    }
}
