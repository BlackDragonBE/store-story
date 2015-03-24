using System;
using UnityEngine;

[Serializable]
public class StockItem
{
    public enum StockCategory
    {
        FruitAndVeg, Candy, Electronics, Dairy, BathAndBody,
        BakedGoods, FrozenGoods, Drinks, Meat, Accessories, Gifts
    }

    public string Name;
    public int Amount;
    public int MaxAmount;
    public float Price;
    [Multiline]
    public string Description;
    public StockCategory Category;
    public string SpriteName;
    public int UnlockLevel;
}