using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseController : MonoBehaviour
{
    public static DatabaseController databaseManager;

    string GetHighScoreTableURL = "https://vesta.uclan.ac.uk/~napoole/FetchHighScores.php";
    string SetHighScoreTableURL = "https://vesta.uclan.ac.uk/~napoole/SetHighScores.php"; 

    [Serializable]
    public class HighScore
    {
       public string Name;
       public int Score;

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }

    }

    [Serializable]
    public class HighScoresList
    {
        public List<HighScore> highScoresList;

        public HighScoresList()
        {
            highScoresList = new List<HighScore>();
        }
    }

    public HighScoresList highScores = new HighScoresList();

    void Awake()
    {
        if (databaseManager == null)
        {
            databaseManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(GetHighScores());

       for(int i = 0; i < 10; i++)
       {
           HighScore highScore = new HighScore("Empty", 0);
           highScores.highScoresList.Add(highScore);
       }
            


        DontDestroyOnLoad(this);
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }

    public IEnumerator SetHighScores(HighScore highScore) 
    {
        string json = JsonUtility.ToJson(highScore);
        Debug.Log("json: " + json);
        var uwr = new UnityWebRequest(SetHighScoreTableURL, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);    
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);  
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();   
        uwr.SetRequestHeader("Content-Type", "application/json");             
        
        yield return uwr.SendWebRequest();                                              
        Debug.Log("Recieved: " + uwr.downloadHandler.text);
        if (uwr.isNetworkError)                                                         
        {                                                                               
            Debug.Log("Error sending" + uwr.error);                                     
            yield return uwr.error;                                                     

        }
    }


    public IEnumerator GetHighScores()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(GetHighScoreTableURL);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error sending" + uwr.error);
        }
        else
        {
            Debug.Log("Recieved: " + uwr.downloadHandler.text);

            highScores.highScoresList.Clear();
            Debug.Log("List Size = " + highScores.highScoresList.Count);
            highScores = JsonUtility.FromJson<HighScoresList>(uwr.downloadHandler.text);
            Debug.Log("List Size = " + highScores.highScoresList.Count);
        }

    }

}
