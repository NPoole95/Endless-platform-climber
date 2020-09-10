using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject normalBranch;
    [SerializeField]
    GameObject brokenBranch;
    [SerializeField]
    GameObject gameOverCanvas;
    [SerializeField]
    Text scoreValueText;

    [SerializeField]
    SpriteRenderer backgroundImage;
    [SerializeField]
    Sprite blueSkyImage;
    [SerializeField]
    Sprite nightSkyImage;
    [SerializeField]
    Sprite sunsetImage;

    [SerializeField]
    SpriteRenderer playerHat;

    [SerializeField]
    public Tilemap branchTileMap;
    [SerializeField]
    public Tilemap backgroundTileMap;
    [SerializeField]
    public Tilemap brokenBranchTileMap;

    [SerializeField]
    public TileBase branchTile;
    [SerializeField]
    public TileBase branchTile2;
    [SerializeField]
    public TileBase branchTile3;
    [SerializeField]
    public TileBase branchTileBL;
    [SerializeField]
    public TileBase branchTileBR;
    [SerializeField]
    public TileBase backgroundItem1;
    [SerializeField]
    public TileBase backgroundItem2;
    [SerializeField]
    public TileBase backgroundItem3;
    [SerializeField]
    public TileBase backgroundItem4;
    [SerializeField]
    public TileBase backgroundItem5;

    [SerializeField]
    GameObject postProcessing;

    //variables for high score bar
    [SerializeField]
    Image scoreBar;
    float maxScoreBarHeight = 393.0f;
    int currentHighScore;
    int currentTenthScore;
    float barValue;
    [SerializeField]
    Text topTenText;
    [SerializeField]
    Text highScoreText;


    float brokenBranchOffset = 0.2f; //the spacing between the seperate left and right parts of the broken branch

    DatabaseController databaseController;
    FacebookController facebookController;

    GameObject player;
    PlayerController playerController;
    GameObject[] branches;

    GameObject scoreTextGO;
    Text scoreText;

    int sectionLoaded = 1; // this is incremented every time a section is loaded and is used to ensure a section is not loaded multiple times.
    const int sectionSize = 30;
    const float branchSpacing = 0.5f;
    const int branchesPerSection = sectionSize / 3;
    
    //the positions that a branch will be placed at during generation
    float branchXPos;
    float branchYPos;
    float branchZPos = 1.0f;
    //the positions a background item will be placed at during generation
    float backgroundItemXPos;
    float backgroundItemYPos;
    float backgroundItemZPos = 1.0f;

    bool playerDead;



    // Start is called before the first frame update
    void Awake()
    {
        

       switch (Random.Range(0, 3))
        {
            case 0:
                backgroundImage.sprite = blueSkyImage;
                break;
            case 1:
                backgroundImage.sprite = nightSkyImage;
                break;
            case 2:
                backgroundImage.sprite = sunsetImage;
                break;
        }

        postProcessing.SetActive(false);
        playerDead = false;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        facebookController = GameObject.Find("FacebookController").GetComponent<FacebookController>();
        scoreTextGO = GameObject.Find("ScoreValueText");
        scoreTextGO.GetComponent<Text>().text = "0";
        player = GameObject.FindGameObjectWithTag("Player");
        databaseController = GameObject.FindGameObjectWithTag("DatabaseController").GetComponent<DatabaseController>();
        playerHat.sprite = facebookController.playerHatSprite;

        for (int i = 0; i < branchesPerSection; ++i)
        {
            branchXPos = Random.Range(-2.0f, 2.0f);
            branchYPos = player.transform.position.y + (i * (sectionSize / branchesPerSection)) + Random.Range(-0.5f, 0.5f);
           // Instantiate(normalBranch, new Vector3(branchXPos, branchYPos, branchZPos), Quaternion.identity);
            Vector3Int branchPos = branchTileMap.WorldToCell(new Vector3(branchXPos, branchYPos, branchZPos));

            switch (Random.Range(0, -3))
            {
                case 0:
                    branchTileMap.SetTile(branchPos, branchTile);
                    break;
                case -1:
                    branchTileMap.SetTile(branchPos, branchTile2);
                    break;
                case -2:
                    branchTileMap.SetTile(branchPos, branchTile3);
                    break;
            }
        }
        currentHighScore = databaseController.highScores.highScoresList[0].Score;
        currentTenthScore = databaseController.highScores.highScoresList[9].Score;

        //calculations to find the correct position of the tenth place text
        float diff =  ((float)currentTenthScore / (float)currentHighScore) * 100;
        diff = diff * 3.93f;
        float temp = (highScoreText.transform.localPosition.y - 393.0f) + diff;

        topTenText.transform.localPosition = new Vector3(topTenText.transform.localPosition.x, temp , topTenText.transform.localPosition.z);

    }

    // Update is called once per frame
    void Update()
    {
        scoreTextGO.GetComponent<Text>().text = playerController.score.ToString();
        barValue = ((float)playerController.score / (float)currentHighScore) * maxScoreBarHeight;
        scoreBar.rectTransform.sizeDelta = new Vector2(3.3f, barValue);
        

        // check if player has died
        if (player.transform.position.y < (Camera.main.transform.position.y - 4) && playerDead == false)
        {
            playerDead = true;
            scoreValueText.text = playerController.score.ToString();
            gameOverCanvas.SetActive(true);
            postProcessing.SetActive(true);
            // playerController.playerRB.gravityScale = 0.0f;
            // playerController.playerRB.velocity = new Vector2(0.0f, 0.0f);
            playerController.playerRB.gameObject.SetActive(false);

            DatabaseController.HighScore highScore;

            if (facebookController.playerInfo != null)
            {
                highScore = new DatabaseController.HighScore(facebookController.playerInfo.name, playerController.score);            
            }
            else
            {
                highScore = new DatabaseController.HighScore("Player", playerController.score);
            }
            StartCoroutine(databaseController.SetHighScores(highScore));
        }



        // platform generation
        float currentPosition = player.transform.position.y;
        if (currentPosition > (sectionSize * sectionLoaded) - (sectionSize / 2))
        {
            for (int i = 0; i < branchesPerSection; ++i)
            {
                branchXPos = Random.Range(-2.0f, 2.0f);
                branchYPos = (currentPosition + (sectionSize / 2)) + (i * (sectionSize / branchesPerSection)) + Random.Range(-0.5f, 0.5f);
                // Instantiate(normalBranch, new Vector3(branchXPos, branchYPos, branchZPos), Quaternion.identity);
                Vector3Int branchPos = branchTileMap.WorldToCell(new Vector3(branchXPos, branchYPos, branchZPos));

                switch (Random.Range(0, -2))
                {
                    case 0:
                        branchTileMap.SetTile(branchPos, branchTile);
                        break;
                    case -1:
                        branchTileMap.SetTile(branchPos, branchTile2);
                        break;
                    case -2:
                        branchTileMap.SetTile(branchPos, branchTile3);
                        break;
                }

                if (Random.Range(0, 6) == 1)
                {
                    branchXPos = Random.Range(-2.0f, 2.0f);
                    branchYPos += Random.Range(-2.0f, 2.0f);
                    Instantiate(brokenBranch, new Vector3(branchXPos, branchYPos, branchZPos), Quaternion.identity);

                   // branchPos = brokenBranchTileMap.WorldToCell(new Vector3(branchXPos, branchYPos, branchZPos));
                    //brokenBranchTileMap.SetTile(branchPos, branchTileBL);
                    //branchPos = brokenBranchTileMap.WorldToCell(new Vector3(branchXPos + brokenBranchOffset, branchYPos, branchZPos));
                    //brokenBranchTileMap.SetTile(branchPos, branchTileBR);

                }

                if (Random.Range(0, 7) == 1)
                {
                    backgroundItemXPos = Random.Range(-2.0f, 2.0f);
                    backgroundItemYPos = branchYPos + Random.Range(-3.0f, 3.0f);
                    Vector3Int backgroundItemPos = backgroundTileMap.WorldToCell(new Vector3(backgroundItemXPos, backgroundItemYPos, backgroundItemZPos));

                    switch (Random.Range(0, -5))
                    {
                        case 0:
                            backgroundTileMap.SetTile(backgroundItemPos, backgroundItem1);
                            break;
                        case -1:
                            backgroundTileMap.SetTile(backgroundItemPos, backgroundItem2);
                            break;
                        case -2:
                            backgroundTileMap.SetTile(backgroundItemPos, backgroundItem3);
                            break;
                        case -3:
                            backgroundTileMap.SetTile(backgroundItemPos, backgroundItem4);
                            break;
                        case -4:
                            backgroundTileMap.SetTile(backgroundItemPos, backgroundItem5);
                            break;                          
                    }
                }
            }
            ++sectionLoaded;
        }


        // platform cleanup
        branches = GameObject.FindGameObjectsWithTag("Branch");
        foreach (GameObject branch in branches)
        {
            if (branch.transform.position.y < Camera.main.transform.position.y - 4)
            {
                Destroy(branch);
            }
        }
        branches = GameObject.FindGameObjectsWithTag("BrokenBranch");
        foreach (GameObject branch in branches)
        {
            if (branch.transform.position.y < Camera.main.transform.position.y - 4)
            {
                Destroy(branch);
            }
        }


    }
}
