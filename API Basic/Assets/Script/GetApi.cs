using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetApi : MonoBehaviour
{
    private InputField outputAre;

    private void Start()
    {
        outputAre = GameObject.Find("Output Area").GetComponent<InputField>();
        GameObject.Find("Get Button").GetComponent<Button>().onClick.AddListener(getData);
    }

    private void getData() => StartCoroutine(getData_Corountine());
    private IEnumerator getData_Corountine()
    {
        outputAre.text = "Loading...";
        string uri = "http://localhost:3000/comments";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                outputAre.text = request.error;
            }else
            {
                outputAre.text = request.downloadHandler.text;
            }
        }
    }
}
