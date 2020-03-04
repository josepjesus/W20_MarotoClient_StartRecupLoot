using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    // Cached references
    public InputField emailInputField;
    public InputField passwordInputField;
    public Player player;

    public delegate void LoginAction();
    public static event LoginAction OnLogin;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        if (!string.IsNullOrEmpty(player.Id))
        {
            OnLogin?.Invoke();
        }
    }

    public void OnLoginButtonClicked()
    {
        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin()
    {
        yield return Helper.InitializeToken(emailInputField.text, passwordInputField.text);
        yield return Helper.GetPlayerInfo();

        if (!string.IsNullOrEmpty(player.Id))
        {
            if (OnLogin != null)
            {
                OnLogin();
            }
        }
        else
        {
            throw new Exception("Login: Error " + player.Name);
        }

    }

}
