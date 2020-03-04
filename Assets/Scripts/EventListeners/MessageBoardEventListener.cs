using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoardEventListener : MonoBehaviour
{
    Player player;

    private void OnEnable()
    {
        Login.OnLogin += Login_OnLogin;
        Logout.OnLogout += Logout_OnLogout;
    }

    private void Logout_OnLogout(string message)
    {
        gameObject.GetComponent<Text>().text += $"\n{message} Bye bye {player.Name}.";
    }

    private void OnDisable()
    {
        Login.OnLogin -= Login_OnLogin;
        Logout.OnLogout -= Logout_OnLogout;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Login_OnLogin()
    {
        gameObject.GetComponent<Text>().text += "\nWelcome " + player.Name + ". You are logged in!";
    }

}
