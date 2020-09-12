using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour
{
    private List<int> cards; // リストの宣言
    // 追加 s //
    private int[] playerhand;
    // 追加 e //

    public bool isGameDeck;

    public bool HasCards
    {
        get { return cards != null && cards.Count > 0; }
    }

    // デリゲート
    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;

    // カード枚数をカウントする
    public int CardCount
    {
        get
        {
            if (cards == null)
            {
                return 0;
            }
            else
            {
                return cards.Count;
            }
        }
    }

    // 戻り値に列挙可能なリストを持つメソッド
    // カード情報を取得する　他のクラスからコール可能
    public IEnumerable<int> GetCards()
    {
        // cardsの中の要素それぞれについて
        foreach (int i in cards)
        {
            yield return i;
        }
    }

    // カードをドローする
    public int Pop()
    {
        int temp = cards[0]; // tempへリスト0番のカードを代入
        cards.RemoveAt(0); // リスト0番のカードを削除

        // CardRemovedが空じゃない場合
        if (CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp)); // メンバ変数CardRemovedにtempを代入
        }

        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card); // 自身のリストにカードを加える

        // CardAddedが空じゃない場合
        if(CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card)); // メンバ変数CardAddedにcardを代入
        }
    }

    // 手札の合計を算出する
    public int HandValue()
    {
        int total = 0;
        int aces = 0;
        //int Rank = 0;

        foreach (int card in GetCards())
        {
            //  cardRank    card
            //      0       2
            //      1       3
            //      2       4
            //      3       5
            //      4       6
            //      5       7
            //      6       8
            //      7       9
            //      8       10
            //      9       11
            //      10      12
            //      11      13
            //      12      1

            // カードの数字を判別(returnはRankで)
            //if(cardRank < 12)
            //{
            //    cardRank += 2;
            //}
            //else
            //{
            //    cardRank = 1;
            //}
            //Rank = cardRank;

            // ブラックジャックでのカードカウント
            int cardRank = card % 13;

            if (cardRank <= 8)
            {
                cardRank += 2;
                total = total + cardRank;
            }
            else if (cardRank > 8 && cardRank < 12)
            {
                cardRank = 10;
                total = total + cardRank;
            }
            else
            {
                aces++;
            }
        }

        // 引いたカードがaceだった場合の処理
        for(int i = 0; i < aces; i++)
        {
            if (total + 11 <= 21)
            {
                total = total + 11;
            }
            else
            {
                total = total + 1;
            }
        }

        return total;
    }

    // カードをシャッフルし、デッキの配列を作る
    public void CreateDeck()
    {
        // cardsを空にする
        cards.Clear();

        // iが52になるまで繰り返しiインクリメント
        for (int i = 0; i < 52; i++)
        {
            // リストにiを加える
            cards.Add(i);
        }

        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); // kは0〜n+1のランダム
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

    public void Reset()
    {
        cards.Clear();
    }

    void Awake()
    {
        // cardsの初期化
        cards = new List<int>();
        // isGameDeckフラグにチェックが付いていたらデッキ配列を作る
        if (isGameDeck)
        {
            CreateDeck();
        }
    }
}
