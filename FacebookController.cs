using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class FacebookController : MonoBehaviour
{
    public static FacebookController facebookController;
    [SerializeField]
    GameObject userNameText;

    [SerializeField]
    GameObject userPicture;

    public PlayerInfo playerInfo;
    public Sprite playerHatSprite;

    void Awake()
    {
        if (facebookController == null)
        {
            facebookController = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if(!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity); // Initializes the Facebook DSK
        }
        else
        {
            FB.ActivateApp(); // THE SDK is already initializes, signals an app activation App event
        }

        DontDestroyOnLoad(gameObject);

        userNameText = GameObject.Find("UserNameText");
        userPicture = GameObject.Find("UserProfilePic");

    }

    void Update()
    {
        userNameText = GameObject.Find("UserNameText");
        userPicture = GameObject.Find("UserProfilePic");
        if (playerInfo != null)
        {
            Debug.Log("working");
            userNameText.GetComponent<Text>().text = playerInfo.name;
            userPicture.GetComponent<SpriteRenderer>().sprite = Sprite.Create(playerInfo.pic, new Rect(0, 0, 50, 50), new Vector2(0, 0));
        }
    }

    void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed ti  initialize the Facebook SDK");
        }
    }

    void OnHideUnity(bool isGameShown)
    {
        if(!isGameShown)
        {
            Time.timeScale = 0; //Pause the game - we will need to hide
        }
        else
        {
            Time.timeScale = 1; // Resume the game - we're getting focus again
        }
    }

    public void LoginButtonPressed()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    void AuthCallback(ILoginResult result)
    {
        if(FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken; //AccessToken class will have session details
            //Debug.Log(aToken.UserId); //Print current access token's User ID
        }
        else
        {
            Debug.Log("User cancelled login");
        }

        FB.API("/me?fields=id,name,email", HttpMethod.GET, DealWithInfoResponse);
        FB.API("/me/picture", HttpMethod.GET, DealWithPicResponse);
    }

    void DealWithInfoResponse(IGraphResult result)
    {
        playerInfo = JsonUtility.FromJson<PlayerInfo>(result.RawResult);
        userNameText.GetComponent<Text>().text = playerInfo.name;
    }

    void DealWithPicResponse(IGraphResult theResult)
    {
        if(string.IsNullOrEmpty(theResult.Error))
        {
            Debug.Log("Recieved texture with resolution " + theResult.Texture.width + " x " + theResult.Texture.height);
            userPicture.GetComponent<SpriteRenderer>().sprite = Sprite.Create(theResult.Texture, new Rect(0, 0, 50, 50), new Vector2(0, 0));
            playerInfo.pic = theResult.Texture;
        }
        else
        {
            Debug.LogWarning("recieved error= " + theResult.Error);
        }
    }

    public class PlayerInfo
    {
        public Texture2D pic;
        public string name;
        public string id;
    }
}
