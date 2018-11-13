using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
//using Google.Apis.Dialogflow.v2;

public class DialogflowAPIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Serializable]
    public class TokenClassName
    {
        public string access_token;
    }

    private static IEnumerator GetAccessToken(Action<string> result)
    {
        Dictionary<string, string> content = new Dictionary<string, string>();
        //Fill key and value
        content.Add("grant_type", "client_credentials");
        content.Add("client_id", "login-secret");
        content.Add("client_secret", "secretpassword");

        UnityWebRequest www = UnityWebRequest.Post("https://someurl.com//oauth/token", content);
        //Send request
        yield return www.SendWebRequest();

        if (!www.isNetworkError)
        {
            string resultContent = www.downloadHandler.text;
            TokenClassName json = JsonUtility.FromJson<TokenClassName>(resultContent);

            //Return result
            result(json.access_token);
        }
        else
        {
            //Return null
            result("");
        }
    }
}
