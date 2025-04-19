using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;




public class FirebaseManager : MonoBehaviour
{
    public string GoogleAPI = "948356357501-egcnjj5dkebgh5jv3t1mt4s3i96r054h.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public Text Username, UserEmail;

    public GameObject LoginScreen, ProfileScreen;

    public SceneManager sceneManager;

    private bool authenticationInProgress = false;
    private bool authenticationSuccessful = false;  

   

    private void Awake() {
        configuration = new GoogleSignInConfiguration{
            WebClientId = GoogleAPI,
            RequestIdToken = true,
        };
    }

    private void Start() {
        InitFirebase();
    }

    void InitFirebase() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.FirebaseApp.DefaultInstance.Options.DatabaseUrl = null;
    }

    public void GoogleSignInClick() {
        authenticationInProgress = true;
        authenticationSuccessful = false;

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        
        Debug.Log("Google Sign In Clicked");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);


    }

    private IEnumerator ShowProfileThenHide(float showDuration)
    {
        ProfileScreen.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        ProfileScreen.SetActive(false);
    }
    private IEnumerator DelayedSceneTransition(string sceneName, float delay)
{
     ProfileScreen.SetActive(false);
    yield return new WaitForSeconds(delay);
    sceneManager.SwitchScenes(sceneName);
}

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        
    if (!task.IsFaulted && !task.IsCanceled) {
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(signInTask => {
            if (!signInTask.IsFaulted && !signInTask.IsCanceled) {
                user = auth.CurrentUser;
                Username.text = "Welcome, " + user.DisplayName;
                UserEmail.text = user.Email;
                
                // Mark authentication as successful
                authenticationSuccessful = true;
            }
            authenticationInProgress = false;
        });
    } else {
        authenticationInProgress = false;
    }
    }

    void Update() {
    // Check if authentication just completed successfully
    if (!authenticationInProgress && authenticationSuccessful) {
        authenticationSuccessful = false; // Reset flag to prevent multiple transitions
        Debug.Log("Authentication completed, transitioning to BantuAR scene");
        StartCoroutine(DelayedSceneTransition("BantuAR", 1f));
    }
    }

  

    // private string CheckImageUrl(string url) {
    //     if (!string.IsNullOrEmpty(url)) {
    //         return url;
    //     }
    //     return imageUrl;
    // }

    // IEnumerator LoadImage(string imageUri) {
    //     WWW www = new WWW(imageUri);
    //     yield return www;

    //     UserProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    // }
}