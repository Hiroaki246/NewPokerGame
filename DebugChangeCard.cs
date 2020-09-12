using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    CardFlipper flipper; // CardFlipper(作成したscript)クラスの参照定義
    CardModel cardModel; // CardModel(作成したscript)クラスの参照定義
    int cardIndex = 0;

    public GameObject card;

  private void Awake()
    {
        // cardにアタッチされているCardModelを取得して使用
        cardModel = card.GetComponent<CardModel>();
        // cardにアタッチされているCardModelを取得して使用
        flipper = card.GetComponent<CardFlipper>();
    }

    private void OnGUI()
    {
        // ボタンの作成 と ボタンが押された時の処理
        if(GUI.Button(new Rect(10,10,100,28),"Hit me!"))
        {
            // cardIndex が cardModel.facesのリストの長さ（５２）以上だった場合（最後のカードの時の処理）
            if (cardIndex >= cardModel.faces.Length)
            {
                cardIndex = 0;
                // CardFlipperクラスのFlipCardメソッドをコール
                // flipCardの実行、引数にデッキの最後のカード,現在のカード,インデックスは-1(CardFlipperで裏面レンダーされる）
                flipper.FlipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack, -1);
            }
            else
            {
                if (cardIndex > 0)
                {
                    // CardFlipperクラスのFlipCardメソッドをコール
                    // flipCardの実行、引数に一つ前のカード,現在のカード,インデックス
                    flipper.FlipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                {
                    // CardFlipperクラスのFlipCardメソッドをコール
                    //flipCardの実行、引数に裏面,現在のカード,インデックス
                    flipper.FlipCard(cardModel.cardBack, cardModel.faces[cardIndex], cardIndex);
                }
                cardIndex++;
            }
        }
    }
}
