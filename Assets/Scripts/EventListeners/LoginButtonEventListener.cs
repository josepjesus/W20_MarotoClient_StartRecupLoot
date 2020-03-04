using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginButtonEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        Login.OnLogin += Login_OnLogin;
        Logout.OnLogout += Logout_OnLogout;
    }

    private void OnDisable()
    {
        Login.OnLogin -= Login_OnLogin;
        Logout.OnLogout -= Logout_OnLogout;
    }

    private void Login_OnLogin()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    private void Logout_OnLogout(string message)
    {
        gameObject.GetComponent<Button>().interactable = true;
    }
}
