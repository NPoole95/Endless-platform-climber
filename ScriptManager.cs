using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager scriptManager;

    void Awake()
    {
        if(scriptManager == null)
        {
            scriptManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }
}
