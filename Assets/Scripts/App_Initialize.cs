using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class App_Initialize : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    public GameObject inMenuUI;
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public Button adButton;
    public GameObject player;
    private bool hasGameStarted = false;
    private bool hasSeenRewardedAd = false;

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null;

    void Awake()
    {
        #if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
        // #elif UNITY_EDITOR
        // _adUnitId = _androidAdUnitId;
        #endif
        adButton.interactable = false;

        InitializeAds();
        Shader.SetGlobalFloat("_Curvature",2.0f);
        Shader.SetGlobalFloat("_Trimming",0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    public void PlayButton(){
        LoadAd();
        if(hasGameStarted)
        StartCoroutine(StartGame(1.5f));
        else{
        //LoadAd();
        StartCoroutine(StartGame(1.5f));
        }
    }

    public void PauseGame(){
        player.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
        hasGameStarted=true;
        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        player.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePosition;
        hasGameStarted=true;
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
        if(hasSeenRewardedAd==true)
        {
            adButton.GetComponent<Image>().color=new Color(1,1,1,0.5f);
            adButton.GetComponent<Button>().enabled=false;
        }
    }

    // public void ShowAd(){
    //     hasSeenRewardedAd=true;
    //     StartCoroutine(StartGame(1.0f));
    // }

    IEnumerator StartGame(float waitTime)
    {
        
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        player.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePositionY;
    }


    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }



public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
 
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            adButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            adButton.interactable = true;
        }
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        adButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }
 
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
                hasSeenRewardedAd=true;
                StartCoroutine(StartGame(1.5f));
        }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
    void OnDestroy()
    {
        // Clean up the button listeners:
        adButton.onClick.RemoveAllListeners();
    }



    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
