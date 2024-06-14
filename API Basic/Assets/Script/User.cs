using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string id;
    public string userName;
    public string passWord;

    public User(string id, string userName, string passWord)
    {
        this.id = id;
        this.userName = userName;
        this.passWord = passWord;
    }
}
