using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteExampleController : MonoBehaviour
{
    FacebookController facebookController;
    SettingsController settingsController;

    [SerializeField]
    SpriteRenderer playerSpriteHatExample;

    // Start is called before the first frame update
    void Awake()
    {
        facebookController = GameObject.Find("FacebookController").GetComponent<FacebookController>();
        settingsController = GameObject.Find("SettingsController").GetComponent<SettingsController>();
    }

    public void NextCostume()
    {
        settingsController.playerSpriteNumber++;
    }
    public void PreviousCostume()
    {
        settingsController.playerSpriteNumber--;
    }

    // Update is called once per frame
    void Update()
    {
        playerSpriteHatExample.sprite = facebookController.playerHatSprite;
    }
}
