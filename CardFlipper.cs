using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
    SpriteRenderer spriteRenderer; // SpriteRendererクラスの参照定義
    CardModel model; // CardModel(作成したscript)クラスの参照定義

    public AnimationCurve scaleCurve; // AnimationCurveを外部参照し、インスペクター上のグラフが使用可能
    public float duration = 0.5f;

    private void Awake()
    {
        // cardにアタッチされているSpriteRendererを取得して使用
        spriteRenderer = GetComponent<SpriteRenderer>();
        // cardにアタッチされているCardModelを取得して使用
        model = GetComponent<CardModel>();
    }

    public void FlipCard(Sprite startImage, Sprite endImage, int cardIndex)
    {
        StopCoroutine(Flip(startImage, endImage, cardIndex)); //現在動いているコルーチン（この場合前回のアニメーション処理）を止める
        StartCoroutine(Flip(startImage, endImage, cardIndex)); //新たにコルーチン（この場合今回のアニメーション処理）を始める
        // コルーチンとは、呼び出した関数を途中(yield return)で抜けて、あとで再開できるルーチンのこと
    }

    //コルーチンで動くメソッドFlipの定義 IEnumeratorはコルーチン作成に必要な型
    IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex)
    {
        spriteRenderer.sprite = startImage; // spriteRendererの画像(sprite)にクラスの引数startImageを代入

        float time = 0f;
        // timeが1秒以上になるまで処理を繰り返す
        while(time <= 1f)
        {
            float scale = scaleCurve.Evaluate(time); // timeに対応するAnimationCurveグラフでのScaleの値の代入
            time = time + Time.deltaTime / duration; // time に time+PCの単位時間をdurationで割ったものを代入

            Vector3 localScale = transform.localScale; // localscaleの名でVector3を宣言、現在(前回値？)のlocalscaleを代入
            localScale.x = scale; // scale(AnimationCurve)のx成分をlocalscaleに代入
            transform.localScale = localScale;

            // timeが0.5以上
            if (time >= 0.5f)
            {
                spriteRenderer.sprite = endImage; // spriteRendererの画像(sprite)にクラスの引数endImageを代入
            }

            yield return new WaitForFixedUpdate(); // コルーチンを一旦抜ける
        }

        // cardIndexが-1の時
        if (cardIndex == -1)
        {
            model.ToggleFace(false); // 裏面をレンダーする
        }
        else
        {
            model.cardIndex = cardIndex; // modelのカードインデックスの値をcardIndexとし
            model.ToggleFace(true); // 表面をレンダーする
        }
    }
}
