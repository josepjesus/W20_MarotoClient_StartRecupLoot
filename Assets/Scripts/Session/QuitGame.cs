using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void OnQuitGameButtonClicked()
    {
        if (!string.IsNullOrEmpty(player.Id))
        {
            gameObject.GetComponent<Logout>().OnLogoutButtonClicked();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
