using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scene2 : MonoBehaviour
{
    Button b1,b2;
    public Toggle teacher, student;
	// Use this for initialization
	void Start ()
    {
        b1 = GameObject.FindGameObjectWithTag("first_login").GetComponent<Button>();
        b1.onClick.AddListener(loadscene);

        b2 = GameObject.FindGameObjectWithTag("first_signup").GetComponent<Button>();
        b2.onClick.AddListener(signup);

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

 

    void loadscene()
    {
        SceneManager.LoadScene("Login");
    }

    void signup()
    {
   
        if(teacher.isOn)
        {
            SceneManager.LoadScene("teacher_signup");
        }
        else if(student.isOn)
        {
            SceneManager.LoadScene("student_signup");
        }
    }

}
