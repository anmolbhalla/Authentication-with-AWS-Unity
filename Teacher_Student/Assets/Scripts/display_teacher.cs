using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class display_teacher : MonoBehaviour
{

    Text ids, stud, num;
    string teachdata;
    JArray array1;
    string t1;
    Button b1;
    // Use this for initialization
    void Start()
    {
        ids = GameObject.FindGameObjectWithTag("t_name").GetComponent<Text>();

        num = GameObject.FindGameObjectWithTag("t_mob").GetComponent<Text>();
        num.text = "";

        stud = GameObject.FindGameObjectWithTag("t_stud").GetComponent<Text>();
        stud.text = "";

        b1 = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<Button>();
        b1.onClick.AddListener(redirect);

        teachdata = PlayerPrefs.GetString("dbdata");
        //Debug.Log("Receiving data" + teachdata);

        array1 = JArray.Parse(teachdata);
        foreach (JObject obj in array1.Children<JObject>())
        {
            foreach (JProperty singleProp in obj.Properties())
            {
                string id = singleProp.Name;
                string value = singleProp.Value.ToString();
                if (string.Equals(id, "name"))
                    ids.text = value;
            }
        }


        StartCoroutine(Upload());
    }


    void redirect()
    {
        SceneManager.LoadScene("Login");
    }
    IEnumerator Upload()
    {

        WWWForm form1 = new WWWForm();
        form1.AddField("name", ids.text);

        UnityWebRequest www = UnityWebRequest.Post("http://3.16.4.70:8080/teachers", form1);
        yield return www.SendWebRequest();

        //www.GetResponseHeaders();

        string data = "["+www.downloadHandler.text+"]";
        //Debug.Log("stud" + data);
        array1 = JArray.Parse(data);
        foreach (JObject obj in array1.Children<JObject>())
        {
            foreach (JProperty singleProp in obj.Properties())
            {
                string id = singleProp.Name;
                string value = singleProp.Value.ToString();
                stud.text = stud.text + id + "\n";
                num.text = num.text + value + "\n";
            }
        }




        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }




    }




    void Update()
    {

    }
}
