using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class PostApi : MonoBehaviour
{
    private InputField outputArea;
    private void Start()
    {
        outputArea = GameObject.Find("Output Area").GetComponent<InputField>();
        GameObject.Find("Post Button").GetComponent<Button>().onClick.AddListener(postData);
    }

    private void postData() => StartCoroutine(postData_Corountine());
    private IEnumerator postData_Corountine()
    {
        outputArea.text = "Loading...";
        string uri = "http://localhost:3000/comments";
        WWWForm form = new WWWForm();
        form.AddField("title", "test data");
        using(UnityWebRequest request = UnityWebRequest.Post(uri,form))
        {
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError)
            {
                outputArea.text = request.error;
            }else
            {
                outputArea.text = request.downloadHandler.text;
            }
        }
    }

 
}
