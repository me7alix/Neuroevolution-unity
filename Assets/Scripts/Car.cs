using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [HideInInspector]
    public NeuralNetwork network;
    private int id;
    private float timer;
    private GameObject gm;
    public void Init(int[] netStruct, int id)
    {
        network = new NeuralNetwork(netStruct);
        this.id = id;
    }
    public void Start()
    {
        GameController.instance.spawnedCars[id] = gameObject;
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        int raysCount = 5;
        float fov = Mathf.PI;
        float angle = -transform.eulerAngles.y / 360f * Mathf.PI * 2f + Mathf.PI / 2f;
        double[] inp = new double[raysCount];
        for (int i = 0; i < raysCount; i++)
        {
            float da = i / (raysCount - 1f) * fov + angle - fov / 2f;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, new Vector3(Mathf.Cos(da), 0, Mathf.Sin(da))
                , out hit, 1000, LayerMask.GetMask("Border")))
            {
                Debug.DrawLine(transform.position, hit.point, Color.white);
                inp[i] = Mathf.Clamp(hit.distance, 0f, 10) / 5d;
            }
        }
        network.SetInp(inp);
        network.CalcNeurons();
        var outp = network.GetOutp();
        //transform.position += transform.forward * Time.fixedDeltaTime * (3f + (float)outp[1] * 3f);
        transform.position += transform.forward * Time.fixedDeltaTime * GameController.instance.carSpeed;
        float calcA = (float)(outp[0] - 0.5);
        transform.eulerAngles += new Vector3(0, calcA * 20f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
            GameController.instance.MinusCar(network, gameObject, timer);
        }
    }
}
