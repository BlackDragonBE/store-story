using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Singleton;

    //References

    //Public

    //Private
    private bool _firstTimePlacingObject = true;
    private bool _shownBuyingButtonTut = false;
    private bool _shownBuyingGoodsTut = false;
    private bool _shownPlacingGoodsTut = false;
    private bool _shownOpeningTheStoreTut = false;

    private void Awake()
    {
        Singleton = this;
    }

    public void OnStartPlacingObject()
    {
        if (StoreManager.Singleton.TotalDaysPlayed == 0 && _firstTimePlacingObject)
        {
            _firstTimePlacingObject = false;

            UIManager.Singleton.ShowMessage("Placing shelves", "Drag the shelves to any tile that's lit green to place it!\n" +
                                                               "Cancel this action by pressing the Cancel button in the upper left.");
        }

        if (StoreManager.Singleton.TotalDaysPlayed == 27)
        {
            UIManager.Singleton.ShowMessage("Types of customers", "Looks like you're ready to know about another type of customer!\n" +
                                                                  "Up until now, all customers wanted an item from a certain category.\n" +
                                                                  "From now on, some customers will want a specific item.\n\n" +
                                                                  "Their choice is also affected by the weather.\n" +
                                                                  "\n" +
                                                                  "Good luck!");
        }
    }

    public void OnClosedStore()
    {
        if (StoreManager.Singleton.TotalDaysPlayed == 0)
        {
            UIManager.Singleton.ShowMessage("Your first day is over!", "Congratulations on your first day!\n\nYou can now review your sales and customers.\n" +
                                                                       "Press the Next Day button when you're ready for your next day!");
        }
    }

    public void OnFirstObjectPlaced()
    {
        if (StoreManager.Singleton.TotalDaysPlayed != 0)
        {
            return;
        }

        if (!_shownBuyingButtonTut)
        {


            UIManager.Singleton.ShowMessage("Buying goods 1", "A store can't sell anything with empty shelves!\n" +
                                                            "Time to buy some goods!\n" +
                                                            "\n" +
                                                            "Press the Buy Goods button to buy some stuff!");
            _shownBuyingButtonTut = true;
        }
    }

    public void OnBuyGoodsWindowOpened()
    {
        if (StoreManager.Singleton.TotalDaysPlayed != 0)
        {
            return;
        }

        if (!_shownBuyingGoodsTut)
        {
            UIManager.Singleton.ShowMessage("Buying goods 2", "When customers start coming in, they will want goods from\n" +
                                                              "a certain category.\n" +
                                                              "\n" +
                                                              "For now, buy some fruit and anything else you'd like!");

            _shownBuyingGoodsTut = true;
        }
    }

    public void OnBuyGoodsWindowClosed()
    {
        if (StoreManager.Singleton.TotalDaysPlayed != 0)
        {
            return;
        }

        if (!_shownPlacingGoodsTut)
        {
            UIManager.Singleton.ShowMessage("Placing goods in shelves", "Place the goods in the shelves by pressing on the goods\n" +
                                                                        "slot on a set of shelves.\n" +
                                                                        "\n" +
                                                                        "Select any item and it will be put into the shelves.\n" +
                                                                        "\n" +
                                                                        "We're almost ready to open up!");

            _shownPlacingGoodsTut = true;
        }
    }

    public void OnPlaceGoodsWindowClosed()
    {
        if (StoreManager.Singleton.TotalDaysPlayed != 0)
        {
            return;
        }

        if (!_shownOpeningTheStoreTut)
        {
            UIManager.Singleton.ShowMessage("Opening the store", "You're finally ready to open the store!\n" +
                                                                 "Exciting, isn't it?\n" +
                                                                 "\n" +
                                                                 "Press the Open Store button to open the doors\n" +
                                                                 "of " + StoreManager.StoreName + "!\n" +
                                                                 "\n" +
                                                                 "Watch what customers like, unhappy customers\n" +
                                                                 "leave the store!");

            _shownOpeningTheStoreTut = true;
        }
    }
}