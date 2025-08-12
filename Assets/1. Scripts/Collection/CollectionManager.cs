using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{

    public CustomerCollectionData[] customerCollectionDatas;

    private void Awake()
    {
        customerCollectionDatas = Resources.LoadAll<CustomerCollectionData>("CustomerCollection");
    }

}
// 1 컬렉션 캔버스
// 2 캔버스 여는 버튼,
// 3 손님/와인탭 버튼 컬렉션 서브패널
// 4. 손님 컬렉션 - 패널 이미지 - 손님 이미지 - 이미지를 클릭하면 손님 정보 캔버스
// custoemrcolelctionpanel패널이 커스터머 컬렉션 데이터 배열만큼 씬상에 생성되게 처리하기