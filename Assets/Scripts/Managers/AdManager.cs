using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour {

    const string APP_ID = "ca-app-pub-7742697768427956/1257964329";
    const string VIDEO_ID = "ca-app-pub-3940256099942544/5224354917";

    RewardBasedVideoAd rewardVideo;
    public static AdManager instance;
    // Use this for initialization

    public bool isLoaded;

	void Awake () {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        MobileAds.Initialize(APP_ID);
    }

    private void Start() {
        rewardVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        //rewardVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


        RequestAd();
    }

    private void Update() {
        isLoaded = rewardVideo.IsLoaded();
        if (!isLoaded) RequestAd();
    }

    private Action OnLoadSuccess, OnLoadFail, OnAdClosed;
    private bool adOver = false;

    public void Set(Action onLoadSuccess, Action onLoadFail, Action onAdClosed) {
        OnLoadSuccess = onLoadSuccess; OnLoadFail = onLoadFail; OnAdClosed = onAdClosed;
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
        print("Ad loaded!");
        OnLoadSuccess();
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        print("Ad failed to load with message: " + args.Message);
        OnLoadFail();
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
        print("Ad opened!");
        adOver = false;
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args) {
        print("Ad started!");
        adOver = false;
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
        print("Ad closed!");
        if (adOver) OnAdClosed();
        else print("Didn't see everything.");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args) {
        adOver = true;
        string type = args.Type;
        double amount = args.Amount;
        print("Rewarded " + amount.ToString() + " " + type);
    }

    //public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args) {
    //    print("HandleRewardBasedVideoLeftApplication event received");
    //}


    void RequestAd() {
        if (!rewardVideo.IsLoaded()) {
            AdRequest request = new AdRequest.Builder().Build();
            rewardVideo.LoadAd(request, VIDEO_ID);
        }
    }

    public void ShowAd() {
        if (rewardVideo.IsLoaded()) {
            rewardVideo.Show();
        }

        else {
            //RequestAd();
            print("Not loaded");
        }
    }
}
