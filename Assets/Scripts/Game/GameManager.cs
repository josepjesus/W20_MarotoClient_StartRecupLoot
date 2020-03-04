using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerNameText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public bool isGameActive;

    public Player player;

    public UnityEngine.UI.Text lootRemain;

    //public GameObject leftColumn;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerNameText.text = player.Name + "(" + (DateTime.Now - player.BirthDay).Days / 365 + ")";
        scoreText.text = "Score: " + score.ToString();
        StartCoroutine(StartLoot());
    }

    private IEnumerator StartLoot()
    {

        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/LootRemain/GetLootRemain", "GET");
        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.downloadHandler = new DownloadHandlerBuffer();

        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("Helper > GetPlayerInfo: " + httpClient.error);
        }
        else
        {
            LootSerializable loot = JsonUtility.FromJson<LootSerializable>(httpClient.downloadHandler.text);
            lootRemain.text = loot.points + "";
        }
        httpClient.Dispose();
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
        spawnRate /= difficulty;
        StartCoroutine(SpawnTarget());
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int randomIndex = UnityEngine.Random.Range(0, 4);
            Instantiate(targets[randomIndex]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;
        StartCoroutine(UpdateLootRemain());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator UpdateLootRemain()
    {
        LootSerializable loot = new LootSerializable();
        loot.points = int.Parse(lootRemain.text);
        using (UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/LootRemain/ChangePoints", "POST"))
        {
            string playerData = JsonUtility.ToJson(loot);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(playerData);
            httpClient.uploadHandler = new UploadHandlerRaw(bodyRaw);
            httpClient.SetRequestHeader("Content-Type", "application/json");

            yield return httpClient.SendWebRequest();
            if (httpClient.isNetworkError || httpClient.isHttpError)
            {
                throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
            }

            httpClient.Dispose();
        }
    }
}
