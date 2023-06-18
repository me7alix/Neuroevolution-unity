using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static System.MathF;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public float carSpeed;
    public GameObject car;
    public Transform spawnpoint;
    [HideInInspector]
    public GameObject[] spawnedCars;

    private int numberInGen = 180;
    private int curNum = 0;
    private int numberOfGen = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CreateGeneration(null);
        spawnedCars = new GameObject[numberInGen];
    }
    void CreateGeneration(NeuralNetwork n)
    {
        curNum = numberInGen;
        numberOfGen++;
        for (int i = 0; i < numberInGen; i++)
        {
            var netStruct = new int[] { 5, 4, 6, 1 };
            GameObject clone = Instantiate(car, spawnpoint);
            var sc = clone.GetComponent<Car>();
            sc.Init(netStruct, i);
            if (n == null)
                sc.network.RandomWeights(10f, 5f);
            else
            {
                if (i < 0)
                    sc.network = n.CopyW(0, 0);
                else if (i < 20)
                {
                    sc.network = n.CopyW(3f, 1f);
                    clone.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                }
                else if (i < 40)
                {
                    sc.network.RandomWeights(10f, 5f);
                    clone.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    sc.network = n.CopyW(0.75f, 0.2f);
                    clone.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                }
            }
        }
    }
    NeuralNetwork best;
    float maxScore = 0f;
    public void MinusCar(NeuralNetwork n, GameObject car, float timer)
    {
        //float score = Vector3.Distance(car.transform.position, spawnpoint.position) + 10f / timer * 0.1f;
        //float score = Vector3.Distance(car.transform.position, spawnpoint.position) - Pow(Pow(timer, E), 0.06f);
        float score = Vector3.Distance(car.transform.position, spawnpoint.position) - 0.2f * timer;
        if (score > maxScore)
        {
            maxScore = score;
            best = n;
        }
        if (--curNum == 0)
        {
            for(int i = 0; i < numberInGen; i++)
                Destroy(spawnedCars[i]);
            CreateGeneration(best);
        }
    }
    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 200, 100),
         $"Number of generations: {numberOfGen}\nNumber of cars: {curNum} / {numberInGen}\nBest: {maxScore}");
    }
}