using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using UnityEngine.Networking;

public class teacher_signup : MonoBehaviour
{
    InputField name, username, password, email, number;
    public Text txt;
    Button signup;
    // Use this for initialization
    void Start()
    {
        signup = GameObject.FindGameObjectWithTag("teacher_button").GetComponent<Button>();
        signup.onClick.AddListener(sendToDatabase);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void showToast(string text, int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }



    void sendToDatabase()
    {
        name = GameObject.FindGameObjectWithTag("teacher_name").GetComponent<InputField>();
        username = GameObject.FindGameObjectWithTag("teacher_user").GetComponent<InputField>();
        password = GameObject.FindGameObjectWithTag("teacher_pass").GetComponent<InputField>();
        email = GameObject.FindGameObjectWithTag("teacher_email").GetComponent<InputField>();
        number = GameObject.FindGameObjectWithTag("teacher_mobile").GetComponent<InputField>();
        //Debug.Log("Hello " + name.text + username.text + password.text + email.text + number.text);

        StartCoroutine(Upload());

    }
    private IEnumerator showToastCOR(string text, int duration)
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
        if (name.text != "" && username.text != "" && email.text != "" && password.text != "" && number.text != "")
        {

            WWWForm form = new WWWForm();
            form.AddField("name", name.text);
            form.AddField("username", username.text);
            form.AddField("email", email.text);
            form.AddField("pass", password.text);
            form.AddField("mobile", number.text);
            form.AddField("type", "teacher");




            UnityWebRequest www = UnityWebRequest.Post("http://3.16.4.70:8080/signup", form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }


            SceneManager.LoadScene("Login");
        }

        else
        {
            showToast("Please Enter All Credentials", 1);
        }


    }
}
