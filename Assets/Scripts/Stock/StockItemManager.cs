using System.Collections.Generic;
using UnityEngine;

public class StockItemManager : MonoBehaviour
{
    public static StockItemManager Singleton;

    //References

    //Public
    public List<StockItem> PossibleItems = new List<StockItem>();
    public List<Sprite> InventorySprites = new List<Sprite>();
    public Sprite UnknownItemSprite;

    public List<ItemCategorySprite> CategorySprites = new List<ItemCategorySprite>();
    //Private

    void Awake()
    {
        Singleton = this;
        AddAllStockItems();
    }

    void Start()
    {
    }

    private void AddAllStockItems()
    {
        //FRUIT & VEGETABLES
        AddStockItem("Apples", 45, "Fresh sweet apples.", StockItem.StockCategory.FruitAndVeg, "apple", 2, 1);
        AddStockItem("Bananas", 35, "", StockItem.StockCategory.FruitAndVeg, "banana", 2.5f, 1);
        AddStockItem("Carrots", 25, "", StockItem.StockCategory.FruitAndVeg, "carrot", 1.8f, 1);
        AddStockItem("Lettuce", 30, "", StockItem.StockCategory.FruitAndVeg, "lettuce", 1.6f, 1);

        AddStockItem("Strawberries", 35, "", StockItem.StockCategory.FruitAndVeg, "strawberry", 4f, 2);
        AddStockItem("Grapes", 45, "", StockItem.StockCategory.FruitAndVeg, "grapes", 3f, 2);
        AddStockItem("Pumpkins", 25, "", StockItem.StockCategory.FruitAndVeg, "pumpkin", 9f, 2);

        AddStockItem("Tomatoes", 50, "", StockItem.StockCategory.FruitAndVeg, "tomato", 3f, 3);
        AddStockItem("Truffles", 20, "", StockItem.StockCategory.FruitAndVeg, "truffle", 50, 3);
        AddStockItem("Durian", 15, "", StockItem.StockCategory.FruitAndVeg, "durian", 80f, 3);

        //DAIRY
        AddStockItem("Milk", 25, "", StockItem.StockCategory.Dairy, "milk", 1.5f, 1);
        AddStockItem("Eggs", 40, "", StockItem.StockCategory.Dairy, "egg", 1f, 1);
        AddStockItem("Cheese", 28, "", StockItem.StockCategory.Dairy, "cheese", 5f, 1);
        AddStockItem("Butter", 50, "", StockItem.StockCategory.Dairy, "butter", 1.2f, 1);

        AddStockItem("Yoghurt", 50, "", StockItem.StockCategory.Dairy, "yoghurt", 3f, 2);
        AddStockItem("Almond Milk", 30, "", StockItem.StockCategory.Dairy, "almond milk", 6f, 2);
        AddStockItem("Whipped Cream", 40, "", StockItem.StockCategory.Dairy, "whipped cream", 8f, 2);

        AddStockItem("Custard", 30, "", StockItem.StockCategory.Dairy, "custard", 8f, 3);
        AddStockItem("Cream Cheese", 25, "", StockItem.StockCategory.Dairy, "cream cheese", 12f, 3);
        AddStockItem("Expensive Cheese", 20, "", StockItem.StockCategory.Dairy, "expensive cheese", 24f, 3);

        //BAKED GOODS
        AddStockItem("Bread", 25, "", StockItem.StockCategory.BakedGoods, "bread", 2f,1);
        AddStockItem("Bagels", 40, "", StockItem.StockCategory.BakedGoods, "bagel", 1f, 1);
        AddStockItem("Croissants", 30, "", StockItem.StockCategory.BakedGoods, "croissant", 3f, 1);
        AddStockItem("Sweet Buns", 25, "", StockItem.StockCategory.BakedGoods, "buns", 5f, 1);

        AddStockItem("Donuts", 35, "", StockItem.StockCategory.BakedGoods, "donut", 4f, 2);
        AddStockItem("Muffins", 30, "", StockItem.StockCategory.BakedGoods, "muffin", 6f, 2);
        AddStockItem("Cake", 15, "", StockItem.StockCategory.BakedGoods, "cake", 9f, 2);

        AddStockItem("Belgian Waffles", 40, "", StockItem.StockCategory.BakedGoods, "waffle", 5f, 3);
        AddStockItem("Pie", 35, "", StockItem.StockCategory.BakedGoods, "pie", 12f, 3);
        AddStockItem("Scones", 50, "", StockItem.StockCategory.BakedGoods, "scone", 8f, 3);

        //CANDY
        AddStockItem("Sweets", 40, "", StockItem.StockCategory.Candy, "sweets", 0.6f,1);
        AddStockItem("Chewing Gum", 60, "", StockItem.StockCategory.Candy, "gum", 0.7f, 1);
        AddStockItem("Chocolate Bar", 30, "", StockItem.StockCategory.Candy, "chocolate", 1f, 1);
        AddStockItem("Cookies", 50, "", StockItem.StockCategory.Candy, "cookie", 1f, 1);

        AddStockItem("Lollipops", 60, "", StockItem.StockCategory.Candy, "lollipop", 2f, 2);
        AddStockItem("Chocolate Eggs", 45, "", StockItem.StockCategory.Candy, "chocolate egg", 3f, 2);
        AddStockItem("Toffees", 60, "", StockItem.StockCategory.Candy, "toffee", 4f, 2);

        AddStockItem("Happy Ranchers", 50, "", StockItem.StockCategory.Candy, "happy rancher", 8f, 3);
        AddStockItem("Candy Hearts", 25, "", StockItem.StockCategory.Candy, "candy heart", 20f, 3);
        AddStockItem("Rare Candy", 10, "", StockItem.StockCategory.Candy, "rare candy", 100f, 3);

        //ELECTRONICS
        AddStockItem("Batteries", 30, "", StockItem.StockCategory.Electronics, "battery", 3f, 1);
        AddStockItem("USB Sticks", 25, "", StockItem.StockCategory.Electronics, "usb stick", 12f,1);
        AddStockItem("Recharger", 20, "", StockItem.StockCategory.Electronics, "charger", 30f, 1);
        AddStockItem("Hair Straightener", 25, "", StockItem.StockCategory.Electronics, "hair straightener", 45f, 1);

        AddStockItem("Video Game", 30, "", StockItem.StockCategory.Electronics, "game", 70f, 2);
        AddStockItem("Kitchen Robot", 20, "", StockItem.StockCategory.Electronics, "blender", 95f, 2);
        AddStockItem("Laptop", 10, "", StockItem.StockCategory.Electronics, "laptop", 600f, 2);

        AddStockItem("Bluray", 60, "", StockItem.StockCategory.Electronics, "bluray", 10f, 3);
        AddStockItem("Toasters", 25, "", StockItem.StockCategory.Electronics, "toaster", 120f, 3);
        AddStockItem("Computers", 10, "", StockItem.StockCategory.Electronics, "pc", 1600f, 3);

        //BATH AND BODY
        AddStockItem("Tissues", 40, "", StockItem.StockCategory.BathAndBody, "tissues", .5f, 1);
        AddStockItem("Toilet Paper", 35, "", StockItem.StockCategory.BathAndBody, "toilet paper", 2f, 1);
        AddStockItem("Lipstick", 20, "", StockItem.StockCategory.BathAndBody, "lipstick", 8f,1);
        AddStockItem("Shower Gel", 50, "", StockItem.StockCategory.BathAndBody, "shower gel", 6f, 1);

        AddStockItem("Toothpaste", 50, "", StockItem.StockCategory.BathAndBody, "toothpaste", 5f, 2);
        AddStockItem("Shampoo", 30, "", StockItem.StockCategory.BathAndBody, "shampoo", 12f, 2);
        AddStockItem("Hair Spray", 36, "", StockItem.StockCategory.BathAndBody, "hair spray", 7f, 2);

        AddStockItem("Mascara", 40, "", StockItem.StockCategory.BathAndBody, "mascara", 18f, 3);
        AddStockItem("Body Lotion", 50, "", StockItem.StockCategory.BathAndBody, "body lotion", 15f, 3);
        AddStockItem("Deodorant", 60, "", StockItem.StockCategory.BathAndBody, "deodorant", 8f, 3);

        //Frozen Goods
        AddStockItem("Crushed Ice", 60, "", StockItem.StockCategory.FrozenGoods, "ice", .5f, 1);
        AddStockItem("Ice Cream", 80, "", StockItem.StockCategory.FrozenGoods, "ice cream", .8f,1);
        AddStockItem("Frozen Pizza", 30, "", StockItem.StockCategory.FrozenGoods, "pizza", 4f, 1);
        AddStockItem("Frozen Vegetables", 40, "", StockItem.StockCategory.FrozenGoods, "frozen vegetables", 6f, 1);

        AddStockItem("Fishsticks", 60, "", StockItem.StockCategory.FrozenGoods, "fishsticks", 4f, 2);
        AddStockItem("Belgian Fries", 50, "", StockItem.StockCategory.FrozenGoods, "fries", 8f, 2);
        AddStockItem("Frozen Yoghurt", 40, "", StockItem.StockCategory.FrozenGoods, "yoghurt", 10f, 2);

        AddStockItem("Calamaries", 20, "", StockItem.StockCategory.FrozenGoods, "calamaries", 15f, 3);
        AddStockItem("Snowman", 40, "", StockItem.StockCategory.FrozenGoods, "snowman", 25f, 3);
        AddStockItem("Walt Disney", 15, "", StockItem.StockCategory.FrozenGoods, "mickey", 500f, 3);

        //DRINKS
        AddStockItem("Water", 30, "", StockItem.StockCategory.Drinks, "water", 1f,1);
        AddStockItem("Soda", 48, "", StockItem.StockCategory.Drinks, "soda", 2.5f, 1);
        AddStockItem("Coffee", 25, "", StockItem.StockCategory.Drinks, "coffee", 4f, 1);
        AddStockItem("Hot Cocoa", 35, "", StockItem.StockCategory.Drinks, "hot cocoa", 6f, 1);

        AddStockItem("Energy Drinks", 80, "", StockItem.StockCategory.Drinks, "energydrink", 2f, 2);
        AddStockItem("Beer", 160, "", StockItem.StockCategory.Drinks, "beer", 2.5f, 2);
        AddStockItem("Special Tea", 35, "", StockItem.StockCategory.Drinks, "tea", 12f, 2);

        AddStockItem("Eggnogg", 45, "", StockItem.StockCategory.Drinks, "eggnogg", 28f, 3);
        AddStockItem("Rum", 50, "", StockItem.StockCategory.Drinks, "alcohol", 25f, 3);
        AddStockItem("Champagne", 30, "", StockItem.StockCategory.Drinks, "champagne", 60f, 3);

        //MEAT
        AddStockItem("Hot Dogs", 40, "", StockItem.StockCategory.Meat, "hot dog", .8f, 1);
        AddStockItem("Salami", 30, "", StockItem.StockCategory.Meat, "salami", 2f, 1);
        AddStockItem("Bacon", 35, "", StockItem.StockCategory.Meat, "bacon", 3.5f, 1);
        AddStockItem("Hamburgers", 25, "", StockItem.StockCategory.Meat, "hamburger", 5f, 1);

        AddStockItem("Sausages", 80, "", StockItem.StockCategory.Meat, "sausage", 4f, 2);
        AddStockItem("Chicken Wings", 60, "", StockItem.StockCategory.Meat, "chicken wings", 10f, 2);
        AddStockItem("Turkey", 20, "", StockItem.StockCategory.Meat, "turkey", 45f, 2);

        AddStockItem("Steak", 50, "", StockItem.StockCategory.Meat, "steak", 8f, 3);
        AddStockItem("Spare Ribs", 80, "", StockItem.StockCategory.Meat, "spare ribs", 2f, 3);
        AddStockItem("A horse", 15, "", StockItem.StockCategory.Meat, "horse", 2000f, 3);

        //ACCESSORIES
        AddStockItem("Sunglasses", 25, "", StockItem.StockCategory.Accessories, "sunglasses", 2f, 1);
        AddStockItem("Gloves", 35, "", StockItem.StockCategory.Accessories, "gloves", 6f, 1);
        AddStockItem("Scarfs", 40, "", StockItem.StockCategory.Accessories, "scarf", 10f, 1);
        AddStockItem("Necklaces", 10, "", StockItem.StockCategory.Accessories, "necklace", 80f, 1);

        AddStockItem("Clown Nose", 50, "", StockItem.StockCategory.Accessories, "nose", 8f, 2);
        AddStockItem("Umbrella", 45, "", StockItem.StockCategory.Accessories, "umbrella", 7f, 2);
        AddStockItem("Parasol", 30, "", StockItem.StockCategory.Accessories, "parasol", 8.5f, 2);

        AddStockItem("Fox Mask", 20, "", StockItem.StockCategory.Accessories, "fox mask", 1500f, 3);
        AddStockItem("Crown", 12, "", StockItem.StockCategory.Accessories, "crown", 4200f, 3);
        AddStockItem("The One Ring", 1, "", StockItem.StockCategory.Accessories, "ring", 50000f, 3);

        //Gifts
        AddStockItem("Flowers", 36, "", StockItem.StockCategory.Gifts, "flowers", .8f, 1);
        AddStockItem("Stuffed Animal", 40, "", StockItem.StockCategory.Gifts, "teddy", 5f, 1);
        AddStockItem("Get Well Card", 60, "", StockItem.StockCategory.Gifts, "get well card", 3f, 1);
        AddStockItem("Birthday Card", 50, "", StockItem.StockCategory.Gifts, "birthday card", 3f, 1);

        AddStockItem("Toy Train", 45, "", StockItem.StockCategory.Gifts, "toy train", 8f, 2);
        AddStockItem("Board Game", 35, "", StockItem.StockCategory.Gifts, "board game", 10f, 2);
        AddStockItem("Perfume", 20, "", StockItem.StockCategory.Gifts, "perfume", 60f, 2);

        AddStockItem("Christmas Sweater", 35, "", StockItem.StockCategory.Gifts, "christmas sweater", 35f, 3);
        AddStockItem("Watch", 15, "", StockItem.StockCategory.Gifts, "watch", 300f, 3);
        AddStockItem("Cat Statue", 8, "", StockItem.StockCategory.Gifts, "cat statue", 1000f, 3);
    }

    private void AddStockItem(string name, int amount, string description, StockItem.StockCategory category, string spriteName, float price, int unlockLevel)
    {
        StockItem newStockItem = new StockItem();
        newStockItem.Name = name;
        newStockItem.Description = description;
        newStockItem.MaxAmount = amount;
        newStockItem.Amount = amount;
        newStockItem.Category = category;
        newStockItem.SpriteName = spriteName;
        newStockItem.Price = price;
        newStockItem.UnlockLevel = unlockLevel;

        PossibleItems.Add(newStockItem);
    }

    public StockItem GetStockItemCopy(string name, float priceModifier = 1.0f)
    {
        StockItem newStockItem = new StockItem();
        StockItem stockItemToCopy = PossibleItems.Find(item => item.Name == name);

        newStockItem.Amount = stockItemToCopy.Amount;
        newStockItem.MaxAmount = stockItemToCopy.MaxAmount;
        newStockItem.Category = stockItemToCopy.Category;
        newStockItem.Description = stockItemToCopy.Description;
        newStockItem.Name = stockItemToCopy.Name;
        newStockItem.SpriteName = stockItemToCopy.SpriteName;
        newStockItem.Price = stockItemToCopy.Price * priceModifier;
        newStockItem.UnlockLevel = stockItemToCopy.UnlockLevel;

        return newStockItem;
    }

    public Sprite GetInventorySprite(string spriteName)
    {
        Sprite spr = InventorySprites.Find(sprite => sprite.name == spriteName);

        if (spr == null)
        {
            spr = UnknownItemSprite;
        }

        return spr;
    }

    public Sprite GetInventorySpriteByItemName(string itemName)
    {
        Sprite spr = GetInventorySprite(PossibleItems.Find(item => item.Name == itemName).SpriteName);

        if (spr == null)
        {
            return UnknownItemSprite;
        }

        return spr;
    }

    public Sprite GetCategorySprite(StockItem.StockCategory category)
    {
        Sprite spr = CategorySprites.Find(sprite => sprite.Category == category).Sprite;

        if (spr == null)
        {
            spr = UnknownItemSprite;
        }

        return spr;
    }
}