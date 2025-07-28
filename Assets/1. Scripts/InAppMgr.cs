using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class InAppMgr : MonoBehaviour, IStoreListener
{
    //InAppMgr 싱글톤 추가
    public static InAppMgr Instance;

    void Awake()
    {
        Instance = this;
    }

    private IStoreController controller;
    private IExtensionProvider extensions;

    public void Start()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //상품 등록
        builder.AddProduct("1000acorn", ProductType.Consumable);
        builder.AddProduct("5000acorn", ProductType.Consumable);
        builder.AddProduct("10gatchaCoin", ProductType.Consumable);
        builder.AddProduct("50gatchaCoin", ProductType.Consumable);


        // 마켓 별 productId 다른 경우
        //builder.AddProduct("gold_1000", ProductType.Consumable, new IDs
        //    {
        //        {"gold_1000_android", GooglePlay.Name}, // Google Play에 등록된 상품 등록
        //        {"gold_1000_ios", MacAppStore.Name} // App Store Connect에 등록된 상품 등록 
        //    });

        UnityPurchasing.Initialize(this, builder);
    }


    // ★★★ 상품 ID로 가격 정보 가져오기 ★★★
    public string GetProductPrice(string productId)
    {
        if (controller != null)
        {
            Product product = controller.products.WithID(productId);
            if (product != null && product.metadata != null)
            {
                if (product.metadata.isoCurrencyCode == "KRW")
                {
                    return $"{product.metadata.isoCurrencyCode} {product.metadata.localizedPrice:F0}";
                }
                else
                {
                    return $"{product.metadata.isoCurrencyCode} {product.metadata.localizedPrice:F2}";
                }

            }
        }
        return "-";
    }

    // 인앱 결제 초기화 
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    // 초기화 실패 시 호출
    public void OnInitializeFailed(InitializationFailureReason error)
    {

    }

    //★★★ 인앱 상품 구매시도 ★★★
    Action<bool> purchaseCallback;
    //인앱 상품 구매시도 
    public void Purchase(string productId, Action<bool> endCb)
    {
        purchaseCallback = endCb;
        controller.InitiatePurchase(productId);
    }

    // 구매 완료 시 호출
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        purchaseCallback?.Invoke(true);
        purchaseCallback = null;
        return PurchaseProcessingResult.Complete;
    }

    // 구매 실패 시 호출
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        purchaseCallback?.Invoke(false);
        purchaseCallback = null;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {

    }
}