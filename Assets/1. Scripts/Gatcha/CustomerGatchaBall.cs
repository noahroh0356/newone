using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGatchaBall : GatchaBall
{
    public SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }


    //SetGatchaBall함수 재정의하기
    public override void SetGatchaBall(string key) // key로 가챠볼이 나타내는 
    {
        base.SetGatchaBall(key); // 부모 클래(가챠)에 있는 SetGatchaBall 함수호출
        //
        Debug.Log("CustomerGatchaBall SetGatchaBall()" + key);
        Debug.Log("spriteRenderer.Lenth" + spriteRenderers.Length);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {

            if (spriteRenderers[i].gameObject.name == key) // 커스터머 매니저 키랑 오브젝트 이름이랑 동일하게 하기
            {
                spriteRenderers[i].gameObject.SetActive(true);
            }

            else
            {
                spriteRenderers[i].gameObject.SetActive(false);
            }
        }
    }



}
