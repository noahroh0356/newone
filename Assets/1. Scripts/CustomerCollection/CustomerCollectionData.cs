using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerCollectionData", menuName = "Game/CustomerCollection")]

public class CustomerCollectionData : ScriptableObject
{
    public string customer1;

    public string customer2;

    public string customer3;

    public RewardType rewardType; // 도토리, 가챠 코인  
    public string rewardValue;// 개수- 100,1 


}

public enum RewardType
{
Coin,
GatchaCoin
}