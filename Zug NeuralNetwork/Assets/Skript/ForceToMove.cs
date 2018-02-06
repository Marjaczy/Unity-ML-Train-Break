using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceToMove : MonoBehaviour
{
    public float acceleration = 80f; //Beschleunigun
    private float breakForce;   //Bremskraft

    private bool FreeRun = false;
    private bool Break = false;

    float[] inputs = new float[1]; //unsicher ob variable nicht in Funktion stehen muss

    private bool initialized = false;
    private Transform Stoßplatte;


    private NeuralNetwork net;
    private Rigidbody rBody;
    private Material[] mats;

    void Update()
    {
        //Run
        //if (FreeRun == false && Break == false)
        //{
        //    GetComponent<Rigidbody>().AddForce(transform.right * acceleration, ForceMode.VelocityChange);
        //    GetComponent<Rigidbody>().useGravity = true;
        //}
        ////Break
        //if (Break == true)
        //{
        //    GetComponent<Rigidbody>().AddForce(-transform.right * breakForce, ForceMode.VelocityChange);
        //    GetComponent<Rigidbody>().useGravity = true;
        //    inputs[0] = breakForce;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //If hit coliderbox "FreeRun" turn Break off
        if (other.gameObject.CompareTag("Free Run"))
        {
            FreeRun = true;
            Break = false;
        }

        //If hit coliderbox "Break" turn break on
        if (other.gameObject.CompareTag("Break"))
        {
            Break = true;
        }
    }

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        mats = new Material[transform.childCount];
        for (int i = 0; i < mats.Length; i++)
            mats[i] = transform.GetChild(i).GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        if (initialized == true)
        {
            if (FreeRun == false && Break == false)
            {
                GetComponent<Rigidbody>().AddForce(transform.right * acceleration, ForceMode.VelocityChange);
                GetComponent<Rigidbody>().useGravity = true;
            }
            //Break
            if (Break == true)
            {
                GetComponent<Rigidbody>().AddForce(-transform.right * breakForce, ForceMode.VelocityChange);
                GetComponent<Rigidbody>().useGravity = true;
                inputs[0] = breakForce;
            }

            //float distance = Vector3.Distance(rBody.position, Stoßplatte.position);
            //if (distance <= 50f)
            //{
            //    print("angekommen");
            //    //Fehlerbehaftet
            //}



            //Aus Update Möglicherweise übernehmen



            float[] outputs = net.FeedForward(inputs);      //speichert vorhergehendes Ergebnis 

            net.AddFitness((1f - Mathf.Abs(inputs[0])));    //Fitness zwischen 0 - 1 

            Init(net);
        }

    }

    public void Init(NeuralNetwork net) //Anstatt Stoßplatte muss hier Bremskraft eingestellt werden
    {
        //this.breakForce = breakForce;
        this.net = net;
        initialized = true;
    }



    //to do:    -Bremskraft vom System verwendet
    //          -Bremskraft regulierbar
    //          -Populationsize wird nicht wirklich benötigt 
    //          -NN richtig konfigurieren
    //          -GUI Geschwindigkeits- & Beschleunigungsmessung
    //          -Abstand zum Vorgehenden Partner muss ebenfalls positives Ergebnis auswerfen
    //          -Füllgrad >= 60% ???

}
