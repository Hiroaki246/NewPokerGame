using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent を使ったスクリプトをゲームオブジェクトにアタッチすると、
// 必要なコンポーネントが自動的にそのゲームオブジェクトに加えられるようになる
// 下記はCardStackクラスの参照をできるようにしている
[RequireComponent(typeof(CardStack))]
public class CardStackView : MonoBehaviour
{
    CardStack deck; // CardStackクラスの参照定義
    Dictionary<int, CardView> fetchedCards; // カードを格納するディクショナリーを宣言
    // Dictionary<Key, Value>

    public Vector3 start; // 最初のカードの位置
    public float cardOffset; // カードをずらす幅
    public bool faceUp = false; // カードの表裏を表すフラグ
    public bool reverseLayerOrder = false; // デッキのレイヤーを逆にするフラグ
    public GameObject cardPrefab; // インスタンスするプレハブ

    // カードの表裏を切り替える
    public void Toggle(int card,bool isFaceUp)
    {
        // ディクショナリーの引数をKeyとする要素の表裏を切り替える
        fetchedCards[card].IsFaceUp = isFaceUp;
    }

    public void Clear()
    {
        deck.Reset();

        foreach(CardView view in fetchedCards.Values)
        {
            Destroy(view.Card);
        }
        fetchedCards.Clear();
    }

    // カードを生成し具現化する
    void Awake()
    {
        fetchedCards = new Dictionary<int, CardView>(); // 新しいディクショナリーを生成
        deck = GetComponent<CardStack>(); // dealer,playerにアタッチされているCardStackを取得して使用
        ShowCards(); // カードを表示する

        deck.CardRemoved += deck_CardRemoved;
        deck.CardAdded += deck_CardAdded;
    }

    // デリゲートによって変数として宣言
    // カードの表示位置の決定とAddCardへジャンプ
    void deck_CardAdded(object sender, CardEventArgs e)
    {
        // オフセット幅の計算
        float co = cardOffset * deck.CardCount;
        Vector3 temp = start + new Vector3(co, 0f); // tempというオフセットした位置の計算
        AddCard(temp, e.CardIndex, deck.CardCount);
    }

    // デッキのカードを他へ移動する
    // デリゲートによって変数として宣言
    void deck_CardRemoved(object sender, CardEventArgs e)
    {
        if (fetchedCards.ContainsKey(e.CardIndex))
        {
            Destroy(fetchedCards[e.CardIndex].Card); // カードのゲームオブジェクトを削除する
            fetchedCards.Remove(e.CardIndex); // ディクショナリーから引数をKeyとする要素を削除する
        }
    }
    
    private void Update()
    {
        ShowCards();
    }

    // カードを表示する
    public void ShowCards()
    {
        int cardCount = 0; // カウンタ

        if (deck.HasCards)
        {
            foreach (int i in deck.GetCards())
            {
                // オフセット幅の計算
                float co = cardOffset * cardCount;
                Vector3 temp = start + new Vector3(co, 0f); // tempというオフセットした位置の計算
                AddCard(temp, i, cardCount);
                cardCount++;
            }
        }
    }

    // カードのクローンを生成する
    void AddCard(Vector3 position,int cardIndex,int positionalIndex)
    {
        // 引数のKeyがディクショナリーに存在する場合
        if (fetchedCards.ContainsKey(cardIndex))
        {
            // faceUpがfalseの場合
            if (!faceUp)
            {
                // 引数のKeyと合う要素に格納してあるゲームオブジェクト「Card」にアタッチされているCardModelを取得して使用
                CardModel model = fetchedCards[cardIndex].Card.GetComponent<CardModel>();
                // カードの表か裏かを決める（引数による）
                model.ToggleFace(fetchedCards[cardIndex].IsFaceUp);
            }
            return;
        }

        GameObject cardCopy = Instantiate(cardPrefab); // カードプレハブのコピー
        cardCopy.transform.position = position; // 現在の位置にposition代入

        CardModel cardModel = cardCopy.GetComponent<CardModel>(); // コピーしたカードプレハブのcardModelクラスを取得
        cardModel.cardIndex = cardIndex; // 引数のカードをcardModel.cardIndexに代入
        cardModel.ToggleFace(faceUp); // カードをレンダーする

        SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>(); // cardCopyのSpriteRendererを取得
        // デッキの表示を逆転するかどうか
        if (reverseLayerOrder)
        {
            spriteRenderer.sortingOrder = 51 - positionalIndex;
        }
        else
        {
            spriteRenderer.sortingOrder = positionalIndex;
        }

        fetchedCards.Add(cardIndex,new CardView(cardCopy));

        //Debug.Log("Hand Value = " + deck.HandValue());
    }
}
