using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Player player;

    public delegate void LogoutAction(string message);
    public static event LogoutAction OnLogout;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }


    public void OnLogoutButtonClicked()
    {
        TryLogout();
    }

    private void TryLogout()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Account/Logout", "POST");
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.SendWebRequest();
        while (!httpClient.isDone)
        {
            Task.Delay(1);
        }

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("Login > TryLogout: " + httpClient.error);
        }
        else
        {
            //if (OnLogout != null)
            //{
            //    OnLogout();
            //}
            OnLogout?.Invoke("" + httpClient.responseCode);
            player.Token = string.Empty;
            player.Id = string.Empty;
            player.Email = string.Empty;
            player.Name = string.Empty;
            player.BirthDay = DateTime.MinValue;
        }
    }

}
