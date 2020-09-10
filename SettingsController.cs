using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static SettingsController settingsController;
    int PlayerCostume;

    [SerializeField]
    FacebookController facebookController;

    [SerializeField]
    Sprite playerSprite0;
    [SerializeField]
    Sprite playerSprite1;
    [SerializeField]
    Sprite playerSprite2;
    [SerializeField]
    Sprite playerSprite3;



    public int playerSpriteNumber = 0;
    // Start is called before the first frame update
    void Awake()
    {
        facebookController = GameObject.Find("FacebookController").GetComponent<FacebookController>();

        if (settingsController == null)
        {
            settingsController = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    { 
        if(playerSpriteNumber > 3)
        {
            playerSpriteNumber = 0;
        }
        else if ( playerSpriteNumber < 0)
        {
            playerSpriteNumber = 3;
        }

        switch (playerSpriteNumber)
        {
            case 0:
                facebookController.playerHatSprite = playerSprite0;
                break;
            case 1:               
                facebookController.playerHatSprite = playerSprite1;
                break;
            case 2:
                facebookController.playerHatSprite = playerSprite2;
                break;
            case 3:
                facebookController.playerHatSprite = playerSprite3;
                break;
        } 
    }

}
