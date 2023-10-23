using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core.Environments;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    private MenuController controller;

    [SerializeField]
    GameObject noAds_icon;

    public string noAds_id;

    [SerializeField]
    Button[] shopOffer_buttons;
    [SerializeField]
    string[] offers_id;

    public static bool noAds_bought;
    public static int token_value;

    const string k_Environment = "production";

    private void Awake()
    {
        controller = GetComponent<MenuController>();

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
        {
            noAds_bought = true;

            noAds_icon.SetActive(false);

            Debug.Log(noAds_id + " purchased");
        }
    }

    public void OnPurchaseComplete(UnityEngine.Purchasing.Product product)
    {
        int token = PlayerPrefs.GetInt("token", 0);

        if (product.definition.id == noAds_id)
        {
            Debug.Log("noAds purchase complete");

            PlayerPrefs.SetInt("adsRemoved", 1);

            noAds_icon.SetActive(false);
        }      

        if (product.definition.id == offers_id[0]) token += 250;
        if (product.definition.id == offers_id[1]) token += 600;
        if (product.definition.id == offers_id[2]) token += 1500;
        if (product.definition.id == offers_id[3]) token += 5000;

        PlayerPrefs.SetInt("token", token);
        controller.UpdateToken();

        Debug.Log(product.definition.id + " token purchased");
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason reason)
    {
        Debug.Log(product + " || " + reason);
    }

    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }

    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        Debug.Log(text);
    }

    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        Debug.LogError(text);
    }
}
