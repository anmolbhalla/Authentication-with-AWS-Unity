using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class display_stud : MonoBehaviour {

    Text name,teachers,marks,email,number;
    string dbdata;
    JArray array;
    string t1;
    Button b1;
	// Use this for initialization
	void Start ()
    {
        name = GameObject.FindGameObjectWithTag("stud_name").GetComponent<Text>();
        

        marks = GameObject.FindGameObjectWithTag("stud_marks").GetComponent<Text>();
        

        email = GameObject.FindGameObjectWithTag("stud_email").GetComponent<Text>();
        

        number = GameObject.FindGameObjectWithTag("stud_mob").GetComponent<Text>();

        b1 = GameObject.FindGameObjectWithTag("Finish").GetComponent<Button>();
        b1.onClick.AddListener(redirect);

        teachers = GameObject.FindGameObjectWithTag("stud_teach").GetComponent<Text>();
        teachers.text = "";


        dbdata = PlayerPrefs.GetString("dbdata");
       // Debug.Log("Receiving data" + dbdata);
        
        array = JArray.Parse(dbdata);
        foreach (JObject obj in array.Children<JObject>())
        {
            foreach (JProperty singleProp in obj.Properties())
            {
                string id = singleProp.Name;
                string value = singleProp.Value.ToString();
                if (string.Equals(id, "name"))
                    name.text = value;
                else if (string.Equals(id, "marks"))
                    marks.text = value;
                else if (string.Equals(id, "email"))
                    email.text = value;
                else if (string.Equals(id, "mobile"))
                    number.text = value;
                else if (string.Equals(id, "teachers"))
                {
                    t1 = value.ToString();
                    t1=t1.Trim('[');
                    t1=t1.Trim(']');
                    string[] ele = t1.Split(',');
                    Debug.Log("splitted" + ele[0]);
                    for(int i = 0; i < ele.Length; i++)
                    {
                        ele[i]=ele[i].Trim('"');
                        Debug.Log("" + ele[i]);
                    }
                    string final="";
                    for (int i = 0; i < ele.Length; i++)
                    {
                        final += ele[i];
                    }
                    teachers.text = final;
                    
                }
            }
        }



    }

    // Update is called once per frame
    void Update () {
		
	}

    void redirect()
    {
        SceneManager.LoadScene("Login");
    }
}
