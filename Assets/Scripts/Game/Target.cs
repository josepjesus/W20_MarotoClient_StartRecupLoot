//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12.0f, maxSpeed = 16.0f,
        maxTorque = 10.0f, xRange = 4.0f, ySpawnPos = -5.0f;
    private GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosionParticle;

    private Text lootRemaining;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque());
        transform.position = RandomSpawnPos();

        lootRemaining = GameObject.Find("RemainingLoot").GetComponent<Text>();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos, 0f);
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            gameManager.UpdateScore(pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            //Debug.Log(gameObject.name + " " + pointValue);
            Destroy(gameObject);
            int result = int.Parse(lootRemaining.text) - pointValue;
            lootRemaining.text = result + "";
            
            if(result <= 0)
            {
                gameManager.GameOver();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
        Destroy(gameObject);
    }

}
