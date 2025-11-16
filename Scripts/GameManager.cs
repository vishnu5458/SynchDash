using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool IsGameStart = false;
    public int score = 0;

    [SerializeField] Transform[] platforms;

    [SerializeField] GameObject[] collectables;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] ParticleSystem[] collectableVFX;

    [SerializeField] Transform playerArea;
    [SerializeField] Transform ghostArea;

    [SerializeField] GameObject playerOBJ;
    [SerializeField] GameObject ghostOBJ;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gamePlayPanel;

    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] TextMeshProUGUI gpScoreText;
    [SerializeField] TextMeshProUGUI goScoreText;

    private List<GameObject> collectablesPool = new List<GameObject>();
    private List<GameObject> obstaclesPool = new List<GameObject>();
    private List<ParticleSystem> collectablesVFXPool = new List<ParticleSystem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        collectablesPool = collectables.ToList();
        obstaclesPool = obstacles.ToList();
        collectablesVFXPool = collectableVFX.ToList();
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        startButton.onClick.AddListener(OnStartGame);
        exitButton.onClick.AddListener(OnExitGame);
        restartButton.onClick.AddListener(OnStartGame);
        mainMenuButton.onClick.AddListener(OnExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStartGame()
    {
        IsGameStart = true;
        score = 0;
        UpdateScore(0);
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        platforms[0].position = new Vector3(0, -1, -100);
        platforms[1].position = new Vector3(0, -1, 0);
        platforms[2].position = new Vector3(0, -1, 100);
        playerOBJ.transform.localPosition = new Vector3(0, 0, -42.0f);
        ghostOBJ.transform.localPosition = new Vector3(0, 0, -42.0f);
        foreach (GameObject go in collectablesPool)
            go.SetActive(false);
        foreach (GameObject go in obstaclesPool)
            go.SetActive(false);

        InvokeRepeating(nameof(SetCollectbleOrObstacle), 0f, 3.5f);
    }

    void OnExitGame()
    {
        IsGameStart = false;
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        CancelInvoke(nameof(SetCollectbleOrObstacle));
    }

    void SetCollectbleOrObstacle()
    {
        if (UnityEngine.Random.Range(0, 5) == 0)
        {
            GameObject obstacle = GetObstacle();
            obstacle.transform.parent = playerArea;
            obstacle.SetActive(true);
            obstacle.transform.localPosition = new Vector3(playerOBJ.transform.localPosition.x, 0, playerOBJ.transform.localPosition.z + 50);

            //GameObject obstacleGhost = GetObstacle();
            //obstacleGhost.SetActive(true);
            //obstacleGhost.transform.parent = ghostArea;
            //obstacleGhost.transform.localPosition = ghostOBJ.transform.localPosition + new Vector3(0, 0, 50);
        }
        else
        {
            GameObject collectable = GetCollectable();
            collectable.transform.parent = playerArea;
            collectable.SetActive(true);
            collectable.transform.localPosition = new Vector3(playerOBJ.transform.localPosition.x, 0, playerOBJ.transform.localPosition.z + 50);

            //GameObject collectableGhost = GetCollectable();
            //collectableGhost.transform.parent = ghostArea;
            //collectableGhost.SetActive(true);
            //collectableGhost.transform.localPosition = ghostOBJ.transform.localPosition + new Vector3(0, 0, 50);
        }
    }

    GameObject GetObstacle()
    {
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            if (!obstaclesPool[i].activeInHierarchy)
            {
                return obstaclesPool[i];
            }
        }

        GameObject go = Instantiate(obstacles[0]);
        obstaclesPool.Add(go);
        return go;
    }

    GameObject GetCollectable()
    {
        for (int i = 0; i < collectablesPool.Count; i++)
        {
            if (!collectablesPool[i].activeInHierarchy)
            {
                return collectablesPool[i];
            }
        }

        GameObject go = Instantiate(collectables[0]);
        collectablesPool.Add(go);
        return go;
    }

    public ParticleSystem GetCollectableVFX()
    {
        for (int i = 0; i < collectablesVFXPool.Count; i++)
        {
            if (!collectablesVFXPool[i].isPlaying)
            {
                return collectablesVFXPool[i];
            }
        }

        GameObject go = Instantiate(collectableVFX[0].gameObject);
        collectablesVFXPool.Add(go.GetComponent<ParticleSystem>());
        return go.GetComponent<ParticleSystem>();
    }

    public void ResetPlatforms()
    {
        Transform _trans = platforms.OrderBy(trans => trans.position.z).First();
        _trans.position += new Vector3(0, 0, 300);
    }

    public void UpdateScore(int value)
    {
        score += value;
        gpScoreText.text = "Score: " + score;
    }

    public void OnGameOver()
    {
        IsGameStart = false;
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        goScoreText.text = "SCORE: " + score;
        CancelInvoke(nameof(SetCollectbleOrObstacle));
    }
}
