using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;

public class UserManager : MonoBehaviour
{
    public User User;
    [SerializeField] private InputField idField;
    [SerializeField] private InputField userNameField;
    [SerializeField] private InputField passwordField;
    const string url = "http://localhost:3000/user";
    const string nameScene = "LoginSuccess";
    private List<User> UserList = new List<User>();
    private ChangeScene changeScene;

    private void Start()
    {
        changeScene = GetComponent<ChangeScene>();
    }
    public void AddUser()
    {
        StartCoroutine(addUser_Corountine(url));
    }

    public void Login()
    {
        StartCoroutine(getUser_Corountine(url));
    }

    public void UpdateUser()
    {
        StartCoroutine(updateUser_Coroutine(url));
    }
    private IEnumerator addUser_Corountine(string url)
    {
        User = new User(idField.text,userNameField.text, passwordField.text);
        string jsonData = JsonUtility.ToJson(User);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    private IEnumerator getUser_Corountine(string url)
    {
        using(UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning(request.error);
            }else
            {
                string json = request.downloadHandler.text;
                try
                {
                    JSONArray jsonArray = JSON.Parse(json).AsArray;
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        JSONNode jsonNode = jsonArray[i];
                        User newUser = new User(jsonNode["id"], jsonNode["userName"], jsonNode["passWord"]);
                        UserList.Add(newUser);
                    }

                    User userValid = UserList.FirstOrDefault(x => x.userName.Equals(userNameField.text) &&
                    x.passWord.Equals(passwordField.text));
                    if (userValid != null)
                    {
                        changeScene.TranslationScene(nameScene);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to deserialize JSON: {e.Message}");
                }
            }
        }
    }

    private IEnumerator updateUser_Coroutine(string url)
    {
        User user = new User(idField.text,userNameField.text,passwordField.text);
        string jsonData = JsonUtility.ToJson(user);
        byte[] byteData = new System.Text.UTF8Encoding().GetBytes(jsonData);
        using(UnityWebRequest request = UnityWebRequest.Put(url+"/"+idField.text, byteData))
        {
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Update user Successful");
            }else
            {
                Debug.Log("Update user Fail");
            }
        }
    }
}
