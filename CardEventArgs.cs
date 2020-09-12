using System;

// デリゲートによってメソッドを変数のように扱える→変数への代入ができる
public class CardEventArgs : EventArgs
{
    public int CardIndex { get; private set; }

    public CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex; // デリゲートのCardIndexに引数を代入
    }
}
