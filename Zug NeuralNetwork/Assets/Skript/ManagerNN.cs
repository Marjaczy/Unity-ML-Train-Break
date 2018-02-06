using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerNN : MonoBehaviour
{
public GameObject TrainPrefab;
public GameObject Stoßplatte;

public int Interval = 5;



private bool isTraning = false;
private int populationSize = 20;
private int generationNumber = 0;
private int[] layers = new int[] { 1, 10, 10, 1 }; //1 input and 1 output
private List<NeuralNetwork> nets;
private bool leftMouseDown = false;
public List<ForceToMove> TrainList = null;

void Timer()
{
    isTraning = false;
}


void Update()
{
    if (isTraning == false)
    {
        if (generationNumber == 0)
        {
            InitTrainNeuralNetworks();
        }
        else
        {
            nets.Sort();
            for (int i = 0; i < populationSize; i++)
            {
                nets[i] = new NeuralNetwork(nets[i]);
                nets[i].Mutate();

                nets[i] = new NeuralNetwork(nets[i]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy
            }

            for (int i = 0; i < populationSize; i++)
            {
                nets[i].SetFitness(0f);
            }
        }


        generationNumber++;

        isTraning = true;
        Invoke("Timer", Interval);
        CreateTrain();
        DestroyTrain();
    }

    /*
    if (Input.GetMouseButtonDown(0))
    {
        leftMouseDown = true;
    }
    else if (Input.GetMouseButtonUp(0))
    {
        leftMouseDown = false;
    }

    if (leftMouseDown == true)
    {
        
    }
    */

}

    

    private void DestroyTrain()
    {
        for (int i = 0; i < TrainList.Count; i++)
        {
            GameObject.Destroy(TrainList[i].gameObject);
        }
    }

private void CreateTrain()
{
    if (TrainList != null)
    {
            DestroyTrain();
    }

    TrainList = new List<ForceToMove>();

    for (int i = 0; i < populationSize; i++)
    {
        ForceToMove train = ((GameObject)Instantiate(TrainPrefab, new Vector3(0f,5f,0f), TrainPrefab.transform.rotation)).GetComponent<ForceToMove>();
        train.Init(nets[i]);
        TrainList.Add(train);
    }

}

void InitTrainNeuralNetworks()
{
    //population must be even
    if (populationSize % 2 != 0)
    {
            populationSize = 20;
    }

    nets = new List<NeuralNetwork>();


    for (int i = 0; i < populationSize; i++)
    {
        NeuralNetwork net = new NeuralNetwork(layers);
        net.Mutate();
        nets.Add(net);
    }

        
}

}
