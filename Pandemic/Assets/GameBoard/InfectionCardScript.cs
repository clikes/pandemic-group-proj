using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionCardScript : MonoBehaviour
{

    public GameObject correspongingCity;
    private GameObject front;
    private bool frontFaced;
    public int cardNumber;


    // Use this for initialization
    void Start()
    {
        front = transform.Find("Front").gameObject;
        frontFaced = false;      //set to false if you want cards to be initially back faced
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Flip()
    {
        if (frontFaced)
        {
            front.SetActive(false);
            frontFaced = false;
        }

        else
        {
            front.SetActive(true);
            frontFaced = true;
        }

    }

    public void Rotate()
    {
        if (transform.eulerAngles.z == 0)
        {
            transform.Rotate(0, 0, -90);
        }

        else
        {
            transform.Rotate(0, 0, 90);
        }
    }
}