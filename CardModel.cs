using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードの作成（表裏が存在するsprite）
public class CardModel : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Sprite[] faces; // カード表面のfacesというsprite型の配列を宣言
    public Sprite cardBack; // カード裏面のcardBackというsuprite型の変数を宣言

    public int cardIndex;

    public void ToggleFace(bool showFace)
    {
        // 本メソッドの引数がtrueなら
        if (showFace) // カードの表を表示
        {
            // facesのcardIndex番目の値をspriteに代入
            spriteRenderer.sprite = faces[cardIndex];
        }
        else // カードの裏を表示
        {
            // cardBackをspriteに代入
            spriteRenderer.sprite = cardBack;
        }
    }

    private void Awake()
    {
        // cardにアタッチされているSpriteRendererを取得して使用
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
