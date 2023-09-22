using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using Unity.VisualScripting;

public class AdController : MonoBehaviour
{
    public static AdController instance;

    private InterstitialAd adInterstitial;
    private RewardedAd adRewarded;
    private BannerView bannerView;
    string idInterstitial, idRewarded, idBanner;

    public static bool rewardedGiven, interstitialGiven, x2Reward, fuelOffer;

    public int[] randomNumb = { 1, 2};
    public int x;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
        }

        idInterstitial = "ca-app-pub-9421503984483424/1113586405";
        idRewarded = "ca-app-pub-9421503984483424/3769338209";
        idBanner = "ca-app-pub-9421503984483424/7453146939";

        MobileAds.Initialize(initStatus =>
        {
            loadRewardedAd(); loadInterstitialAd();
        });

        this.bannerView = new BannerView(idBanner, AdSize.Banner, AdPosition.Bottom);

        var request = new AdRequest();

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 0)
            this.bannerView.LoadAd(request);

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            Debug.Log("ADS BLOCKED NO ADS");
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.instance.isGameOver || gameController.instance.choosingUpgrade)
            {
                if (!interstitialGiven)
                {
                    showAd();
                }
            }
            else
            {
                if (!adInterstitial.CanShowAd())
                {
                    loadInterstitialAd();
                }
            }
        }
    }

    void showAd()
    {
        x = randomNumb[Random.Range(0, randomNumb.Length)];
        Debug.Log("x: " + x);

        if (x == 1)
        {
            showInterstitial();
        }
        else 
        {
            interstitialGiven = true;
        }
    }

    private void loadRewardedAd()
    {
        if (adRewarded != null)
        {
            adRewarded.Destroy();
            adRewarded = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(idRewarded, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              adRewarded = ad;
              RegisterEventHandlers(adRewarded);
          });
    }

    private void loadInterstitialAd()
    {
        if (adInterstitial != null)
        {
            adInterstitial.Destroy();
            adInterstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(idInterstitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                adInterstitial = ad;
            });

        interstitialGiven = false;
    }

    public void showInterstitial()
    {
        if (adInterstitial != null && adInterstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialGiven = true;

            adInterstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void x2RewardedAd()
    {
        if (this.adRewarded.CanShowAd())
        {
            x2Reward = true;

            adRewarded.Show((Reward reward) =>
            {

            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            if (x2Reward)
            {
                rewardedGiven = true;
                x2Reward = false;
            }

            if (fuelOffer)
            {       
                Debug.Log("fuelGiven");
            }
        };
    }
}
