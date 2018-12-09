using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using JsonData;

//using Google.Apis.Dialogflow.v2;

public class DialogflowAPIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // AccessToken is being generated manually in terminal
        //StartCoroutine(GetAgent("ya29.c.ElpfBkjOUlTRSaNDg-i0tBjGc2WlRT9GePIqe1_j5Xq9flXHMGJWnn5sEjNHyG1VfMFqtt3WapHAVo2-RwvPNKRTHI0BkF9OVUzZJ5OWJEILr64_ge1tgcbS7AA"));

        //https://stackoverflow.com/questions/51272889/unable-to-send-post-request-to-dialogflow-404
        //first param is the dialogflow API call, second param is Json web token
        StartCoroutine(PostRequest("https://dialogflow.googleapis.com/v2/projects/test-67717/agent/sessions/34563:detectIntent",
                                  "ya29.c.ElptBu_jlUH4fp7lFmMgPeJ_uFT86xzkLUYaBEljbY6eNUacLjr0xSiie2TweOukhGzZ8KvxGoH1IWtNiNLpmhvnzAjO5mRjV5qekyHggvt6Ua6l1Bq1DoCu9DE"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PostRequest(String url, String AccessToken){
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        RequestBody requestBody = new RequestBody();
        requestBody.queryInput = new QueryInput();
        // This is an audio api query
        //requestBody.queryInput.audioConfig = new InputAudioConfig();
        //requestBody.queryInput.audioConfig.audioEncoding = AudioEncoding.

        // This is a text api query
        requestBody.queryInput.text = new TextInput();
        requestBody.queryInput.text.text = "hello";
        requestBody.queryInput.text.languageCode = "en";

        string jsonRequestBody = JsonUtility.ToJson(requestBody,true);
        Debug.Log(jsonRequestBody);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
       //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if(postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
        }
    }
    IEnumerator GetAgent(String AccessToken)
    {

        UnityWebRequest www = UnityWebRequest.Get("https://dialogflow.googleapis.com/v2/projects/test-67717/agent");
       
        www.SetRequestHeader("Authorization", "Bearer " + AccessToken);

        yield return www.SendWebRequest();
        //myHttpWebRequest.PreAuthenticate = true;
        //myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
        //myHttpWebRequest.Accept = "application/json";

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
    //[Serializable]
    //public class TokenClassName
    //{
    //    public string access_token;
    //}

    //private static IEnumerator GetAccessToken(Action<string> result)
    //{
    //    Dictionary<string, string> content = new Dictionary<string, string>();
    //    //Fill key and value
    //    content.Add("grant_type", "client_credentials");
    //    content.Add("client_id", "login-secret");
    //    content.Add("client_secret", "secretpassword");

    //    UnityWebRequest www = UnityWebRequest.Post("https://someurl.com//oauth/token", content);
    //    //Send request
    //    yield return www.SendWebRequest();

    //    if (!www.isNetworkError)
    //    {
    //        string resultContent = www.downloadHandler.text;
    //        TokenClassName json = JsonUtility.FromJson<TokenClassName>(resultContent);

    //        //Return result
    //        result(json.access_token);
    //    }
    //    else
    //    {
    //        //Return null
    //        result("");
    //    }
    //}
}
