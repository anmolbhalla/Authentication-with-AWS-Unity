using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class help : MonoBehaviour
{

    public bool isOn;
    Toggle teacher1,teacher2,teacher3;
	// Use this for initialization
	void Start ()
    {
        teacher1 = GameObject.FindGameObjectWithTag("teacher_1").GetComponent<Toggle>();

        teacher2 = GameObject.FindGameObjectWithTag("teacher_2").GetComponent<Toggle>();

        teacher3 = GameObject.FindGameObjectWithTag("teacher_3").GetComponent<Toggle>();

    }
	
	void Update ()
    {
    }
}
