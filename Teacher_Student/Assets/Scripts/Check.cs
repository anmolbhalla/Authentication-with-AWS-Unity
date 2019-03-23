using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEditor;

public class Check : MonoBehaviour
{
    public InputField username;
    public InputField password;
    Button login;
    Dropdown d1;
    public Text txt;
    string type = "";
    
    // Use this for initialization
    void Start()
    {
        login = GameObject.FindGameObjectWithTag("login").GetComponent<Button>();
        login.onClick.AddListener(submit_form);

        d1 = GameObject.FindGameObjectWithTag("logindrop").GetComponent<Dropdown>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void showToast(string text,int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }

    void submit_form()
    {

        username = GameObject.FindGameObjectWithTag("username").GetComponent<InputField>();

        password = GameObject.FindGameObjectWithTag("password").GetComponent<InputField>();
        

         StartCoroutine(Upload());
    }
    private IEnumerator showToastCOR(string text,int duration)
    {
        Color orginalColor = txt.color;

        txt.text = text;
        txt.enabled = true;

        //Fade in
        yield return fadeInAndOut(txt, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(txt, false, 0.5f);

        txt.enabled = false;
        txt.color = orginalColor;
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }


    IEnumerator Upload()
    {

        if (username.text!="" && password.text!="")
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username.text);
            form.AddField("pass", password.text);

            if (d1.value == 0)
            {
                type = "teacher";
            }
            else if (d1.value == 1)
            {
                type = "student";
            }
            form.AddField("type", type);

            UnityWebRequest www = UnityWebRequest.Post("http://3.16.4.70:8080/login", form);
            yield return www.SendWebRequest();

            //www.GetResponseHeaders();

            string data = www.downloadHandler.text;
            if (data != "0" && data != "2")
            {
                //Debug.Log("sending" + data);
                PlayerPrefs.SetString("dbdata", data);

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                }

                if (d1.value == 0)
                {
                    SceneManager.LoadScene("Display_teacher");
                }
                else if (d1.value == 1)
                {
                    SceneManager.LoadScene("Display");
                }
            }

            else
            {
                showToast("Please Enter Valid Credentials", 1);
            }
        }

        else
        {
            showToast("No Field Should be Empty", 1);
        }
   }
}
