using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void selectScene()
    {
        
        switch (this.gameObject.name)
        {
            case "PlayButton":
                SceneManager.LoadScene("GameScene");
                break;
            case "InstructionsButton":
                SceneManager.LoadScene("InstructionsScene");
                break;
            case "SettingsButton":
                SceneManager.LoadScene("SettingsScene");
                break;
            case "HighScoreButton":
                SceneManager.LoadScene("HighScoreScene");
                break;
            case "QuitButton":
                /////////////////
                break;
            case "MenuButton":
                SceneManager.LoadScene("MenuScene");
                break;
        }
    }
}

