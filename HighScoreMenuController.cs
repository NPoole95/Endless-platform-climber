using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreMenuController : MonoBehaviour
{
    DatabaseController databaseController;
    void Awake()
    {
        databaseController = GameObject.FindGameObjectWithTag("DatabaseController").GetComponent<DatabaseController>();
        DatabaseController.HighScoresList highScoresList = new DatabaseController.HighScoresList();
        StartCoroutine(databaseController.GetHighScores());
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < databaseController.highScores.highScoresList.Count; i++)
        {
            string nameTextBox = "NameText" + i;
            string scoreTextBox = "ScoreText" + i;

            Text text = GameObject.Find(nameTextBox).GetComponent<Text>();
            text.text = databaseController.highScores.highScoresList[i].Name;

            text = GameObject.Find(scoreTextBox).GetComponent<Text>();
            text.text = databaseController.highScores.highScoresList[i].Score.ToString();

        }
    }
}
