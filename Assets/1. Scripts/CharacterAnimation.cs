using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    public void Start()
    {

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(CoAnimation());

    }

    IEnumerator CoAnimation()
    {
        int idx = 0;
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            spriteRenderer.sprite = sprites[idx];
            yield return wait;
            idx++;
            if (idx >= sprites.Length)
                idx = 0;
        }

    }

}
