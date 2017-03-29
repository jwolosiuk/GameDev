using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("first"))
            DefaultPlayerPrefs();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DefaultPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString("first", "val");
        PlayerPrefs.SetInt("blood", 1);
        PlayerPrefs.SetInt("mute", 0);
        PlayerPrefs.SetFloat("sound", 1);
    }

    public void setMute(bool b)
    {
        if (b)
        {
            PlayerPrefs.SetInt("mute", 1);
            Debug.Log("set mute on");
        }
        else
        {
            PlayerPrefs.SetInt("mute", 0);
            Debug.Log("set mute off");
        }
    }

    public void setSound(float f)
    {
        PlayerPrefs.SetFloat("sound", f);
        Debug.Log(f);
    }

    public void StartGame ()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
