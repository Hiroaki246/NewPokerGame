using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 追加 s //
// 追加 e //

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    // 追加 s //
    public CardStack community;
    // 追加 e //

    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    // 追加 s //
    public Button checkButton;
    // 追加 e //

    public Text winnerText;

    // 追加 s //
    private int[] playerHand;
    private int[] dealerHand;
    private int[] checkHand;
    // 追加 e //

    // 追加 s //
    private string playerValue;
    private string dealerValue;
    // 追加 e //

    // 追加 s //
    private bool playerflag = false;
    private bool dealerflag = false;
    // 追加 e //

    /*
     * Cards dealt to each player
     * First player hits/sticks/bust
     * Dealer`s turn; must have minmum of 17 score hand
     * Dealers cards;first card is hidden, subsequent cards are facing
     */

    #region Public methods

    public void Hit()
    {
        //player.Push(deck.Pop());
        //if (player.HandValue() > 21)
        //{
        //    // TODO: The player is bust
        //    hitButton.interactable = false;
        //    stickButton.interactable = false;
        //    StartCoroutine(DealersTurn());
        //}

        //community.Push(deck.Pop());
        //CheckHandValue(playerHand.Length);
        Check();
        if (community.CardCount >= 5)
        {
            hitButton.interactable = false;
        }
    }

    // 追加 s //
    public void Check()
    {
        playerValue = "null";
        dealerValue = "null";
        if (community.CardCount == 0)
        {
            for (int j = 0; j < 3; j++)
            {
                community.Push(deck.Pop());
            }
            CommunityStack(5);
            playerflag = true;
            Debug.Log("playerhand is ");
            CheckHandValue(5);
            playerflag = false;
            dealerflag = true;
            Debug.Log("dealerhand is ");
            CheckHandValue(5);
            dealerflag = false;
        }
        else if(community.CardCount == 3)
        {
            community.Push(deck.Pop());
            CommunityStack(6);
            playerflag = true;
            Debug.Log("playerhand is ");
            CheckHandValue(6);
            playerflag = false;
            dealerflag = true;
            Debug.Log("dealerhand is ");
            CheckHandValue(6);
            dealerflag = false;
        }
        else if (community.CardCount == 4)
        {
            community.Push(deck.Pop());
            CommunityStack(7);
            playerflag = true;
            Debug.Log("playerhand is ");
            CheckHandValue(7);
            playerflag = false;
            dealerflag = true;
            Debug.Log("dealerhand is ");
            CheckHandValue(7);
            dealerflag = false;
        }

    }
    // 追加 e //

    public void Stick()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        // TODO: Dealer
        StartCoroutine(DealersTurn());
    }

    public void PlayAgain()
    {
        playAgainButton.interactable = false;

        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        deck.GetComponent<CardStackView>().Clear();
        deck.CreateDeck();

        winnerText.text = "";

        hitButton.interactable = true;
        stickButton.interactable = true;

        dealersFirstCard = -1;

        StartGame();
    }

    #endregion

    #region Unity messages

    private void Start()
    {
        StartGame();
    }

    #endregion

    // ゲーム開始時に、プレイヤーとディーラーにカードを２枚配る
    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            // 追加 s //
            //HitDealer();
            dealer.Push(deck.Pop());
            // 追加 e //
        }
        // 追加 s //
        // フロップを表示できるかの確認
        //for(int j = 0; j < 3; j++)
        //{
        //    community.Push(deck.Pop());
        //}
        //CommunityStack();
        //CheckHandValue(5);
        // 追加 e //
    }

    // 追加 s //
    // コミュニティのカードを合わせる
    void CommunityStack(int a)
    {
        playerHand = new int[a];
        dealerHand = new int[a];
        int i = 0;
        foreach (int card in player.GetCards())
        {
            playerHand[i] = card;
            i++;
        }
        foreach (int card in community.GetCards())
        {
            playerHand[i] = card;
            i++;
        }
        int j = 0;
        foreach (int card in dealer.GetCards())
        {
            dealerHand[j] = card;
            j++;
        }
        foreach (int card in community.GetCards())
        {
            dealerHand[j] = card;
            j++;
        }
    }

    void CheckHandValue(int a)
    {
        int[] Number = new int[a]; // 手札の数字
        int[] Suite = new int[a]; // 手札の柄
        int[] pair = new int[13]; // {No2,3,4,5,6,7,8,9,10,11,12,13,1}
        int rankPoint;
        // 同じ数字のカードが何枚あるかカウント
        if (playerflag == true)
        {
            for (int i = 0; i < a; i++)
            {
                Suite[i] = playerHand[i] / 13;
                Number[i] = playerHand[i] % 13;
                Number[i] = Number[i] + 2; // Aは14
            }
        }
        if (dealerflag == true)
        {
            for (int i = 0; i < a; i++)
            {
                Suite[i] = dealerHand[i] / 13;
                Number[i] = dealerHand[i] % 13;
                Number[i] = Number[i] + 2; // Aは14
            }
        }
        var list = new List<int>();
        list.AddRange(Number); // Numberの要素を入れたlistを作成
        list.Sort(); // 小さい順に並べる
        list.Reverse(); // 逆転して大きい順にする

        if (a == 5) // 未完
        {
            HashSet<int> pairChecker = new HashSet<int>() { Number[0], Number[1], Number[2], Number[3], Number[4] };
            // フラッシュ系の場合
            if (Suite[0] == Suite[1] && Suite[1] == Suite[2] && Suite[2] == Suite[3] && Suite[3] == Suite[4])
            {
                int temp = 10;

                // 弱いストレートフラッシュの場合
                if (list[0] == 14 && list[1] == 5 && list[2] == 4 && list[3] == 3 && list[4] == 2)
                {
                    // rankPoint = list[0]*10^4
                    temp = temp * 10;
                    rankPoint = temp * list[0] * 1000;
                }
                // 強いストレートフラッシュの場合
                else if ((list[0] + list[1] + list[2] + list[3] + list[4]) / 5 == list[2] && list[2] - 2 == list[4] && list[2] + 2 == list[0])
                {
                    // rankPoint = list[0]*10^5
                    temp = temp * 100;
                    rankPoint = temp * list[0] * 10000;
                }
                // フラッシュの場合
                else
                {
                    // rankPoint = list[0]*10
                    rankPoint = temp * list[0];
                }
            }
            // フラッシュ以外
            else if (pairChecker.Count < 5)
            {
                int x;
                int y;
                int doubling = 0;
                for (x = 0; x < list.Count; x++)
                {
                    for (y = x + 1; y < list.Count; y++)
                    {
                        if (list[x] == list[y])
                        {
                            doubling++;
                        }
                    }
                }
                if (doubling == 6) // フォーカード
                {
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1])
                        {
                            // rankPoint = list[k]*10^3
                            rankPoint = list[k] * 1000;
                            break;
                        }
                    }
                    Debug.Log("Hand is Fourcard");
                }
                else if (doubling == 4) // フルハウス
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        // rankPoint = list[k]*10^2
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * 100;
                        }
                        else
                        {
                            temp2 = list[k] * 10;
                        }
                    }
                    rankPoint = temp + temp2;
                    Debug.Log("Hand is full house");
                }
                else if (doubling == 3) // スリーカード
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * (-100);
                            break;
                        }
                    }
                    if (list[0] != list[1]) // example 9 8 8 8 7
                    {
                        temp2 = list[0] * (-10);
                    }
                    else if (list[1] != list[2]) // example 9 8 7 7 7
                    {
                        temp2 = list[1] * (-10);
                    }
                    else if (list[2] != list[3]) // example 8 8 8 7 6
                    {
                        temp2 = list[3] * (-10);
                    }
                    rankPoint = temp + temp2;
                    playerValue = "threecard";
                    Debug.Log("Hand is threecard");
                }
                else if (doubling == 2) // ツーペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < 4; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int n;
                    int pairIndexB = 0;
                    int pairNumB;
                    for (n = 0; n < 2; n++)
                    {
                        if (list[n] == list[n + 1]) // 残りの3つの数から連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexB++; // 二個目の小さいペアが存在するインデックスの開始位置
                    }
                    pairNumB = list[pairIndexB]; //ペアのナンバー
                    list.RemoveAll(p => p == list[pairIndexB]);//ナンバーと一致している数を取り除く

                    rankPoint = (pairNumA * (-1000)) + (pairNumB * (-100));
                    playerValue = "twopairs";
                    Debug.Log("Hand is twopairs");
                }
                else if (doubling == 1) // ワンペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < a - 1; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int temp = 0;
                    for (int n = 0; n < a - 2; n++)
                    {
                        temp = temp + list[n];
                    }
                    rankPoint = (pairNumA * (-10000)) + temp * (-1000);
                    playerValue = "onepair";
                    Debug.Log("Hand is onepair");
                }
            }
            // 弱いストレートの場合
            else if (list[0] == 14 && list[1] == 5 && list[2] == 4 && list[3] == 3 && list[4] == 2)
            {
                int temp = 0;
                for(int n = 0; n < a; n++)
                {
                    temp = temp + list[n];
                }
                rankPoint = (temp - 13) * (-10);
                playerValue = "straight";
                Debug.Log("Hand is 5 high straight");
            }
            // 強いストレートの場合
            else if ((list[0] + list[1] + list[2] + list[3] + list[4]) / 5 == list[2] && list[2] - 2 == list[4] && list[2] + 2 == list[0])
            {
                int temp = 0;
                for (int n = 0; n < a; n++)
                {
                    temp = temp + list[n];
                }
                rankPoint = temp * (-10);
                playerValue = "straight";
                Debug.Log("Hand is straight");
            }
            else // ハイカード
            {
                int temp = 0;
                for (int k = 0; k < a; k++)
                {
                    temp = temp + list[k];
                }
                rankPoint = temp * (-100000);
                playerValue = "straight";
                Debug.Log("Hand is highcard");
            }
            Debug.Log("end");
        }
        else if (a == 6) // 未完
        {
            HashSet<int> pairChecker = new HashSet<int>() { Number[0], Number[1], Number[2], Number[3], Number[4], Number[5] };
            HashSet<int> suiteChecker = new HashSet<int>() { Suite[0], Suite[1], Suite[2], Suite[3], Suite[4], Suite[5] };

            // フラッシュ系の場合
            if (suiteChecker.Count < 3)
            {
                int temp = 10;

                // 弱いストレートフラッシュの場合
                if (list[0] == 14 && list[1] == 5 && list[2] == 4 && list[3] == 3 && list[4] == 2)
                {
                    // rankPoint = list[0]*10^4
                    temp = temp * 10;
                    rankPoint = temp * list[0] * 1000;
                }
                // 強いストレートフラッシュの場合
                else if ((list[0] + list[1] + list[2] + list[3] + list[4]) / 5 == list[2] && list[2] - 2 == list[4] && list[2] + 2 == list[0])
                {
                    // rankPoint = list[0]*10^5
                    temp = temp * 100;
                    rankPoint = temp * list[0] * 10000;
                }
                // フラッシュの場合
                else
                {
                    // rankPoint = list[0]*10
                    rankPoint = temp * list[0];
                }
            }
            // フラッシュ以外
            else if (pairChecker.Count < 5)
            {
                int x;
                int y;
                int doubling = 0;
                for (x = 0; x < list.Count; x++)
                {
                    for (y = x + 1; y < list.Count; y++)
                    {
                        if (list[x] == list[y])
                        {
                            doubling++;
                        }
                    }
                }
                if (doubling == 6) // フォーカード
                {
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1])
                        {
                            // rankPoint = list[k]*10^3
                            rankPoint = list[k] * 1000;
                            break;
                        }
                    }
                    Debug.Log("Hand is Fourcard");
                }
                else if (doubling == 4) // フルハウス
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        // rankPoint = list[k]*10^2
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * 100;
                        }
                        else
                        {
                            temp2 = list[k] * 10;
                        }
                    }
                    rankPoint = temp + temp2;
                    Debug.Log("Hand is full house");
                }
                else if (doubling == 3) // スリーカード
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * (-100);
                            break;
                        }
                    }
                    if (list[0] != list[1]) // example 9 8 8 8 7
                    {
                        temp2 = list[0] * (-10);
                    }
                    else if (list[1] != list[2]) // example 9 8 7 7 7
                    {
                        temp2 = list[1] * (-10);
                    }
                    else if (list[2] != list[3]) // example 8 8 8 7 6
                    {
                        temp2 = list[3] * (-10);
                    }
                    rankPoint = temp + temp2;
                    Debug.Log("Hand is threecard");
                }
                else if (doubling == 2) // ツーペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < 4; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int n;
                    int pairIndexB = 0;
                    int pairNumB;
                    for (n = 0; n < 2; n++)
                    {
                        if (list[n] == list[n + 1]) // 残りの3つの数から連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexB++; // 二個目の小さいペアが存在するインデックスの開始位置
                    }
                    pairNumB = list[pairIndexB]; //ペアのナンバー
                    list.RemoveAll(p => p == list[pairIndexB]);//ナンバーと一致している数を取り除く

                    rankPoint = (pairNumA * (-1000)) + (pairNumB * (-100));
                    Debug.Log("Hand is twopairs");
                }
                else if (doubling == 1) // ワンペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < a - 1; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int temp = 0;
                    for (int n = 0; n < a - 2; n++)
                    {
                        temp = temp + list[n];
                    }
                    rankPoint = (pairNumA * (-10000)) + temp * (-1000);
                    Debug.Log("Hand is onepair");
                }
            }
            else // ハイカード
            {
                int temp = 0;
                for (int k = 0; k < a; k++)
                {
                    temp = temp + list[k];
                }
                rankPoint = temp * (-100000);
                Debug.Log("Hand is highcard");
            }
            Debug.Log("end");
        }
        else if (a == 7)
        {
            /*
            * H H H H H H H 6+5+4+3+2+1=21
            * H H H H H H C 5+4+3+2+1=15
            * H H H H H C C 4+3+2+1+0+1=11
            * H H H H H C D 4+3+2+1+0+0=10
            * ----------------------------
            * H H H H C C C 3+2+1+0+2+1=9
            * H H H H C C D 3+2+1+0+1+0=7
            * H H H C C C C 9
            */
            int suitedoubling = 0;
            for (int x = 0; x < 7; x++)
            {
                for (int y = x + 1; y < 7; y++)
                {
                    if (Suite[x] == Suite[y])
                    {
                        suitedoubling++;
                    }
                }
            }

            HashSet<int> pairChecker = new HashSet<int>() { Number[0], Number[1], Number[2], Number[3], Number[4], Number[5], Number[6] };
            // フラッシュ系の場合
            if (suitedoubling > 9)
            {
                var suitelist = new List<int>();

                int countClub = 0;
                int countDiamond = 0;
                int countHeart = 0;
                int countSpard = 0;
                int[] suiteClub = new int[7];
                int[] suiteDiamond = new int[7];
                int[] suiteHeart = new int[7];
                int[] suiteSpard = new int[7];

                // 各スーツの配列に振り分け
                for (int i = 0; i < 7; i++)
                {
                    if (playerflag == true)
                    {
                        if (playerHand[i] < 13)
                        {
                            suiteClub[countClub] = (playerHand[i] % 13) + 2;
                            countClub++;
                        }
                        else if (13 <= playerHand[i] && playerHand[i] < 26)
                        {
                            suiteDiamond[countDiamond] = (playerHand[i] % 13) + 2;
                            countDiamond++;
                        }
                        else if (26 <= playerHand[i] && playerHand[i] < 39)
                        {
                            suiteHeart[countHeart] = (playerHand[i] % 13) + 2;
                            countHeart++;
                        }
                        else
                        {
                            suiteSpard[countSpard] = (playerHand[i] % 13) + 2;
                            countSpard++;
                        }
                    }
                    else if (dealerflag == true)
                    {
                        if (dealerHand[i] < 13)
                        {
                            suiteClub[countClub] = (dealerHand[i] % 13) + 2;
                            countClub++;
                        }
                        else if (13 <= dealerHand[i] && dealerHand[i] < 26)
                        {
                            suiteDiamond[countDiamond] = (dealerHand[i] % 13) + 2;
                            countDiamond++;
                        }
                        else if (26 <= dealerHand[i] && dealerHand[i] < 39)
                        {
                            suiteHeart[countHeart] = (dealerHand[i] % 13) + 2;
                            countHeart++;
                        }
                        else
                        {
                            suiteSpard[countSpard] = (dealerHand[i] % 13) + 2;
                            countSpard++;
                        }
                    }
                }
                // フラッシュが成立しているスーツをsuitelistに代入
                if (countClub >= 4)
                {
                    suitelist.AddRange(suiteClub);
                    suitelist.Sort(); // 小さい順に並べる
                    suitelist.Reverse(); // 逆転して大きい順にする
                }
                else if (countDiamond >= 4)
                {
                    suitelist.AddRange(suiteDiamond);
                    suitelist.Sort(); // 小さい順に並べる
                    suitelist.Reverse(); // 逆転して大きい順にする
                }
                else if (countHeart >= 4)
                {
                    suitelist.AddRange(suiteHeart);
                    suitelist.Sort(); // 小さい順に並べる
                    suitelist.Reverse(); // 逆転して大きい順にする
                }
                else
                {
                    suitelist.AddRange(suiteSpard);
                    suitelist.Sort(); // 小さい順に並べる
                    suitelist.Reverse(); // 逆転して大きい順にする
                }

                int temp = 10;
                // 弱いストレートフラッシュの場合
                if (suitelist[0] == 14 && suitelist[1] == 5 && suitelist[2] == 4 && suitelist[3] == 3 && suitelist[4] == 2)
                {
                    // rankPoint = list[0]*10^4
                    temp = temp * 10;
                    rankPoint = temp * suitelist[0] * 1000;
                }
                // 強いストレートフラッシュの場合
                else if ((suitelist[0] + suitelist[1] + suitelist[2] + suitelist[3] + suitelist[4]) / 5 == suitelist[2] && suitelist[2] - 2 == suitelist[4] && suitelist[2] + 2 == suitelist[0])
                {
                    // rankPoint = list[0]*10^5
                    temp = temp * 100;
                    rankPoint = temp * suitelist[0] * 10000;
                }
                // フラッシュの場合
                else
                {
                    // rankPoint = list[0]*10
                    rankPoint = temp * suitelist[0];
                }
            }
            // フラッシュ以外
            else if (pairChecker.Count < 7)
            {
                /* 4cards
                 * A A A A 2 2 2 3+2+1+0+2+1=9
                 * A A A A 2 2 3 3+2+1+0+1+0=7
                 * full house-----------------
                 * A A A 2 2 2 3 2+1+0+2+1+0=6
                 * A A A 2 2 3 3 2+1+0+1+0+1=5
                 * A A A 2 2 3 4 2+1+0+1+0+0=4
                 * 3cards---------------------
                 * A A A 2 3 4 5 2+1+0+0+0+0=3
                 * 2pairs---------------------
                 * A A 2 2 3 4 5 1+1+0+0+0+0=2
                 * 1pair----------------------
                 * A 2 3 4 5 6 7 =1
                 */
                int x;
                int y;
                int doubling = 0;
                for (x = 0; x < list.Count; x++)
                {
                    for (y = x + 1; y < list.Count; y++)
                    {
                        if (list[x] == list[y])
                        {
                            doubling++;
                        }
                    }
                }
                if (doubling > 6) // フォーカード
                {
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1])
                        {
                            // rankPoint = list[k]*10^3
                            rankPoint = list[k] * 1000;
                            break;
                        }
                    }
                    Debug.Log("Hand is Fourcard");
                }
                else if (doubling > 3 && doubling <= 6) // フルハウス
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        // rankPoint = list[k]*10^2
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * 100;
                        }
                        else
                        {
                            temp2 = list[k] * 10;
                        }
                    }
                    rankPoint = temp + temp2;
                    Debug.Log("Hand is full house");
                }
                else if (doubling == 3) // スリーカード
                {
                    int temp = 0;
                    int temp2 = 0;
                    for (int k = 0; k < a; k++)
                    {
                        if (list[k] == list[k + 1] && list[k + 1] == list[k + 2])
                        {
                            temp = list[k] * (-100);
                            break;
                        }
                    }
                    if (list[0] != list[1]) // example 9 8 8 8 7
                    {
                        temp2 = list[0] * (-10);
                    }
                    else if (list[1] != list[2]) // example 9 8 7 7 7
                    {
                        temp2 = list[1] * (-10);
                    }
                    else if (list[2] != list[3]) // example 8 8 8 7 6
                    {
                        temp2 = list[3] * (-10);
                    }
                    rankPoint = temp + temp2;
                    if (playerflag == true)
                    {
                        playerValue = "threecard";
                    }
                    else if (dealerflag == true)
                    {
                        dealerValue = "threecard";
                    }
                    Debug.Log("Hand is threecard");
                }
                else if (doubling == 2) // ツーペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < 4; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int n;
                    int pairIndexB = 0;
                    int pairNumB;
                    for (n = 0; n < 2; n++)
                    {
                        if (list[n] == list[n + 1]) // 残りの3つの数から連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexB++; // 二個目の小さいペアが存在するインデックスの開始位置
                    }
                    pairNumB = list[pairIndexB]; //ペアのナンバー
                    list.RemoveAll(p => p == list[pairIndexB]);//ナンバーと一致している数を取り除く

                    rankPoint = (pairNumA * (-1000)) + (pairNumB * (-100));
                    if (playerflag == true)
                    {
                        playerValue = "twopairs";
                    }
                    else if (dealerflag == true)
                    {
                        dealerValue = "twopairs";
                    }
                    Debug.Log("Hand is twopairs");
                }
                else if (doubling == 1) // ワンペア
                {
                    int m;
                    int pairIndexA = 0;
                    int pairNumA;
                    for (m = 0; m < a - 1; m++)
                    {
                        if (list[m] == list[m + 1]) // 連番で一致する場所を確認する
                        {
                            break;
                        }
                        pairIndexA++; // 一個目の大きいペアが存在するインデックスの開始位置
                    }
                    pairNumA = list[pairIndexA]; //ペアのナンバー
                    list.RemoveAll(p => p == pairNumA);//ナンバーと一致している数を取り除く

                    int temp = 0;
                    for (int n = 0; n < a - 2; n++)
                    {
                        temp = temp + list[n];
                    }
                    rankPoint = (pairNumA * (-10000)) + temp * (-1000);
                    if (playerflag == true)
                    {
                        playerValue = "onepair";
                    }
                    else if (dealerflag == true)
                    {
                        dealerValue = "onepair";
                    }
                    Debug.Log("Hand is onepair");
                }
            }
            // 弱いストレートの場合
            else if (list[0] == 14 && list[1] == 5 && list[2] == 4 && list[3] == 3 && list[4] == 2)
            {
                int temp = 0;
                for (int n = 0; n < a; n++)
                {
                    temp = temp + list[n];
                }
                rankPoint = (temp - 13) * (-10);
                if (playerflag == true)
                {
                    playerValue = "straight";
                }
                else if (dealerflag == true)
                {
                    dealerValue = "straight";
                }
                Debug.Log("Hand is 5 high straight");
            }
            // 強いストレートの場合
            else if ((list[0] + list[1] + list[2] + list[3] + list[4]) / 5 == list[2] && list[2] - 2 == list[4] && list[2] + 2 == list[0])
            {
                int temp = 0;
                for (int n = 0; n < a; n++)
                {
                    temp = temp + list[n];
                }
                rankPoint = temp * (-10);
                if (playerflag == true)
                {
                    playerValue = "straight";
                }
                else if (dealerflag == true)
                {
                    dealerValue = "straight";
                }
                Debug.Log("Hand is straight");
            }
            else // ハイカード
            {
                int temp = 0;
                for (int k = 0; k < a; k++)
                {
                    temp = temp + list[k];
                }
                rankPoint = temp * (-100000);
                if (playerflag == true)
                {
                    playerValue = "highcard";
                }
                else if (dealerflag == true)
                {
                    dealerValue = "highcard";
                }
                Debug.Log("Hand is highcard");
            }
            Debug.Log("end");
        }
    }
            
        // 追加 e //

        void HitDealer()
    {
        int card = deck.Pop();

        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            // ディーラーのカードを１枚表にする
            view.Toggle(card, true);
        }
    }

    // プレイヤーのターンが終わり、ディーラーがカードの合計が17以上になるまでカードを引く
    IEnumerator DealersTurn()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        yield return new WaitForSeconds(1f);

        while (dealer.HandValue() < 17)
        {
            HitDealer();
            yield return new WaitForSeconds(1f);
        }

        // プレイヤーがバースト または ディーラーが勝利
        if (player.HandValue() > 21 || dealer.HandValue() >= player.HandValue() && dealer.HandValue() <= 21)
        {
            winnerText.text = "Sorry-- you lose";
        }
        // ディーラーがバースト または プレイヤーの勝利
        else if(dealer.HandValue() > 21 || player.HandValue() <= 21 && player.HandValue() > dealer.HandValue())
        {
            winnerText.text = "Winner, winner! Chicken dinner";
        }
        // 引き分け
        else
        {
            winnerText.text = "The house wins!";
        }

        yield return new WaitForSeconds(1f);
        playAgainButton.interactable = true;
    }
}