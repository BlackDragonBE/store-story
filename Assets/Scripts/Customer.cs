using System.Security;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    //References

    //Public
    public Animator CustomerAnimator;
    public Transform LookTarget;

    public SpriteRenderer ThoughtBubble;
    public SpriteRenderer ThoughtSprite;

    public CustomerMood Mood;
    public bool ActuallyInStore;

    //Searching for..
    public TypeOfCustomer CustomerType = TypeOfCustomer.LooksForCategory;
    public StockItem.StockCategory CategorySearchingFor;
    public string SpecificItemNameSearhingFor;

    public GameObject Boy;
    public GameObject Girl;

    //Private
    private Transform _myTransform;
    private bool _followingCustomer;

    private Customer _customerToFollow;

    private float _timeWaiting;
    private float _priceOfAllItemsTaken;

    private GameObject _containerToGetItemFrom = null; //When an interesting container is found, fill this in

    public enum CustomerMood
    {
        Happy, Sad, Neutral
    }

    public enum TypeOfCustomer
    {
        LooksForCategory, NeedsSpecificItem, ImpulseShopper
    }

    void Awake()
    {
        _myTransform = transform;

        //Boy or girl?
        int rnd = Random.Range(0, 2);

        if (rnd == 0)
        {
            Boy.SetActive(true);
            Girl.SetActive(false);

            CustomerAnimator = Boy.GetComponent<Animator>();
        }
        else
        {
            Girl.SetActive(true);
            Boy.SetActive(false);

            CustomerAnimator = Girl.GetComponent<Animator>();
        }

        //Size variety
        float rndSize = Random.Range(.8f, 1f);
        _myTransform.localScale = new Vector3(rndSize, rndSize, rndSize);

        //Set Customer type
        if (StoreManager.Singleton.TotalDaysPlayed > 28)
        {
            int rndType = Random.Range(0, 101);

            if (EconomyManager.Singleton.ExtraPopularItemNames.Count == 0)
            {
                if (rndType < 10)
                {
                    CustomerType = TypeOfCustomer.NeedsSpecificItem;
                }
            }
            else
            {
                if (rndType < 45)
                {
                    CustomerType = TypeOfCustomer.NeedsSpecificItem;
                }
            }

        }
    }

    private void Start()
    {
        SetCategory();

        LookTarget = StoreManager.Singleton.StoreEntrance;
        //Go.to(_myTransform, 2f, new GoTweenConfig().position(StoreManager.Singleton.StoreEntrance.position));
        _myTransform.DOMove(StoreManager.Singleton.StoreEntrance.position, 2f);
        StartCoroutine(StepIntoStore());
    }

    private void SetCategory()
    {
        int rand = Random.Range(0, 101);

        if (rand < 40)
        {
            CategorySearchingFor = EconomyManager.Singleton.TopCategory;
        }
        else if (rand > 40 && rand < 65)
        {
            CategorySearchingFor = EconomyManager.Singleton.SecondCategory;
        }
        else if (rand > 65 && rand < 80)
        {
            CategorySearchingFor = EconomyManager.Singleton.ThirdCategory;
        }
        else if (rand > 80 && rand < 101)
        {
            int index = Random.Range(0, Enum.GetValues(typeof(StockItem.StockCategory)).Length);
            CategorySearchingFor =
                (StockItem.StockCategory)Enum.GetValues(typeof(StockItem.StockCategory)).GetValue(index);
        }
    }

    private void Update()
    {
        if (!ActuallyInStore && !StoreManager.Singleton.StoreIsOpen)
        {
            Destroy(gameObject);
        }

        if (LookTarget != null)
        {
            _myTransform.LookAt(LookTarget);
        }

        if (_followingCustomer && _customerToFollow != null)
        {
            // _myTransform.positionTo(.6f, _customerToFollow._myTransform.position + new Vector3(1,0,0));

            _myTransform.position = Vector3.MoveTowards(_myTransform.position,
    _customerToFollow._myTransform.position + new Vector3(1, 0, 0), 1f * Time.deltaTime);

            //if (CustomerManager.Singleton._customersReadyToPay.Count > 5)
            //{
            //    _myTransform.position = Vector3.MoveTowards(_myTransform.position,
            //        _customerToFollow._myTransform.position + new Vector3(-1, 0, 0), 1f * Time.deltaTime);
            //}
            //else
            //{
            //    _myTransform.position = Vector3.MoveTowards(_myTransform.position,
            //        _customerToFollow._myTransform.position + new Vector3(1, 0, 0), 1f * Time.deltaTime);
            //}

            if (Vector3.Distance(_myTransform.position, _customerToFollow._myTransform.position) < 1.01f)
            {
                CustomerAnimator.SetBool("Walking", false);
            }
            else
            {
                CustomerAnimator.SetBool("Walking", true);
            }

            _timeWaiting += Time.deltaTime;

            if (_timeWaiting >= 20f && Mood == CustomerMood.Happy)
            {
                SetMood(CustomerMood.Neutral);
            }

            if (_timeWaiting >= 35f && Mood == CustomerMood.Neutral)
            {
                SetMood(CustomerMood.Sad);
            }
        }
    }

    private IEnumerator StepIntoStore()
    {
        yield return new WaitForSeconds(2.1f);
        ActuallyInStore = true;
        CustomerManager.Singleton.Customers.Add(this);
        LookTarget = null;

        _myTransform.LookAt(_myTransform.position + new Vector3(1, 0, 0));
        _myTransform.DOMoveX(_myTransform.position.x + 3f, 1f);

        yield return new WaitForSeconds(1.1f);
        CustomerAnimator.SetBool("Walking", false);

        switch (CustomerType)
        {
            case TypeOfCustomer.LooksForCategory:
                ShowCategoryBubble(CategorySearchingFor); //Display thought bubble
                break;
            case TypeOfCustomer.NeedsSpecificItem:
                SetSpecificItemSearchingFor();
                ShowItemBubble(SpecificItemNameSearhingFor);
                break;
            case TypeOfCustomer.ImpulseShopper:
                break;
        }

        yield return new WaitForSeconds(.5f);

        GetInterestingItemContainer();

        if (_containerToGetItemFrom != null)
        {
            CustomerAnimator.SetBool("Walking", true);
            StartCoroutine(WalkToContainerAndGetItem());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SetMood(CustomerMood.Sad);
            StartCoroutine(ExitStore(false));
        }

    }

    private void SetSpecificItemSearchingFor()
    {
        int rndIndex = Random.Range(0, EconomyManager.Singleton.StockItemNamePool.Count);
        SpecificItemNameSearhingFor = EconomyManager.Singleton.StockItemNamePool[rndIndex];
        //Debug.Log(SpecificItemNameSearhingFor);
    }

    /// <summary>
    /// Finds an item container that's interesting depending on the customer type
    /// </summary>
    void GetInterestingItemContainer()
    {
        _containerToGetItemFrom = null;
        List<GameObject> interestingStoreObjects = new List<GameObject>();

        switch (CustomerType)
        {
            case TypeOfCustomer.LooksForCategory:
                //Gets all items of wanted category
                interestingStoreObjects = StoreManager.Singleton.GetContainersWithCategory(CategorySearchingFor);

                if (interestingStoreObjects.Count == 0) //Nothing intersting
                {
                    _containerToGetItemFrom = null;
                    //SetMood(CustomerMood.Sad);
                    //StartCoroutine(ExitStore(false));
                }
                else
                {
                    int randomIndex = Random.Range(0, interestingStoreObjects.Count);
                    _containerToGetItemFrom = interestingStoreObjects[randomIndex];
                }
                break;
            case TypeOfCustomer.NeedsSpecificItem:
                //Gets all items of wanted category
                interestingStoreObjects = StoreManager.Singleton.GetContainersWithSpecificItem(SpecificItemNameSearhingFor);

                if (interestingStoreObjects.Count == 0) //Nothing intersting
                {
                    _containerToGetItemFrom = null;
                    //SetMood(CustomerMood.Sad);
                    //StartCoroutine(ExitStore(false));
                }
                else
                {
                    int randomIndex = Random.Range(0, interestingStoreObjects.Count);
                    _containerToGetItemFrom = interestingStoreObjects[randomIndex];
                }
                break;
            case TypeOfCustomer.ImpulseShopper:
                //Random item in store?
                break;
        }

    }

    private IEnumerator WalkToContainerAndGetItem()
    {
        float distance = GetDistanceToLocation(_containerToGetItemFrom.transform.position);

        LookTarget = _containerToGetItemFrom.transform;

        _myTransform.DOMove(_containerToGetItemFrom.transform.position -
                         _containerToGetItemFrom.transform.up, distance / 2f);
        //Customer getting items

        yield return new WaitForSeconds(distance / 2f);
        CustomerAnimator.SetBool("Walking", false);
        CustomerAnimator.SetTrigger("Pay");


        yield return new WaitForSeconds(Random.Range(.5f, 2f));
        CustomerAnimator.SetBool("Walking", true);

        int amountOfItemsToGet = Random.Range(1, 7);

        if (_containerToGetItemFrom != null && _containerToGetItemFrom.GetComponent<ItemStockContainer>().InventoryItem != null)
        {

            //Limit amount of items to get when there's not enough left in pack
            if (amountOfItemsToGet > _containerToGetItemFrom.GetComponent<ItemStockContainer>().InventoryItem.Amount)
            {
                amountOfItemsToGet =
                    _containerToGetItemFrom.GetComponent<ItemStockContainer>().InventoryItem.Amount;
            }

            _priceOfAllItemsTaken +=
                _containerToGetItemFrom.GetComponent<ItemStockContainer>().InventoryItem.Price * amountOfItemsToGet;
            _containerToGetItemFrom.GetComponent<ItemStockContainer>().TakeItemsOutOfPack(amountOfItemsToGet);
            SoundManager.Singleton.PlayGetItemSound();

            //Ready to pay at register
            CustomerManager.Singleton.ReadyToPay(this);
        }
        else
        {
            //Try again, item may have been purchased just before you
            GetInterestingItemContainer();

            if (_containerToGetItemFrom != null)
            {
                CustomerAnimator.SetBool("Walking", true);
                StartCoroutine(WalkToContainerAndGetItem());
            }
            else
            {
                yield return new WaitForSeconds(1f);
                SetMood(CustomerMood.Sad);
                StartCoroutine(ExitStore(false));
            }
        }



    }

    public IEnumerator GoToRegisterAndExit()
    {
        _followingCustomer = false;
        LookTarget = StoreManager.Singleton.CustomerBuyTransform;

        float distance = GetDistanceToLocation(StoreManager.Singleton.CustomerBuyTransform.position);
        //Go.to(_myTransform, distance/2f, new GoTweenConfig().position(StoreManager.Singleton.CustomerBuyTransform.position + new Vector3(0, 0, 0)));
        _myTransform.DOMove(StoreManager.Singleton.CustomerBuyTransform.position, distance / 2f);
        yield return new WaitForSeconds(distance / 2f - .3f);
        LookTarget = StoreManager.Singleton.StoreClerk;

        CustomerAnimator.SetBool("Walking", false);
        float baseWaitTime = 2.5f;
        if (StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.CashierProductivity1))
        {
            baseWaitTime -= .5f;
        }

        if (StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.CashierProductivity2))
        {
            baseWaitTime -= .5f;
        }

        if (StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.CashierProductivity3))
        {
            baseWaitTime -= .5f;
        }

        CustomerAnimator.SetTrigger("Pay");
        yield return new WaitForSeconds(baseWaitTime);
        SoundManager.Singleton.PlayCashRegisterSound();
        StoreManager.Singleton.Money += _priceOfAllItemsTaken;
        StoreManager.Singleton.Profit += _priceOfAllItemsTaken;
        UIManager.Singleton.ShowFloatingText(_priceOfAllItemsTaken.ToString("C"), new Color(0, .85f, 0, 1), _myTransform.position + new Vector3(0, 2, 0));

        //Exit store
        StartCoroutine(ExitStore(true));
    }

    public IEnumerator ExitStore(bool purchasedItems)
    {
        //Exit store
        LookTarget = StoreManager.Singleton.StoreEntrance;
        yield return new WaitForSeconds(.3f);
        CustomerManager.Singleton.OnCustomerLeft(this, purchasedItems);
        LookTarget = null;
        //Go.to(_myTransform, 1f, new GoTweenConfig().position(new Vector3(-3, 0, 0), true));
        CustomerAnimator.SetBool("Walking", true);

        Tween t;

        if (purchasedItems)
        {
            t = _myTransform.DOMoveX(_myTransform.position.x - 3, 1f);
        }
        else
        {
            //Hopefully fixes customers walking through walls
            t = _myTransform.DOMove(StoreManager.Singleton.StoreEntrance.position - new Vector3(1f, 0, 0), 1f);
            //t = _myTransform.DOMoveX(_myTransform.position.x - 4, 1f);
        }

        yield return t.WaitForCompletion();
        //Go.to(_myTransform, 3f, new GoTweenConfig().position(new Vector3(0, 0, 10), true));
        t = _myTransform.DOMoveZ(_myTransform.position.z + 10, 3f);
        _myTransform.LookAt(_myTransform.position + new Vector3(0, 0, 1));

        yield return t.WaitForCompletion();
        //Go.killAllTweensWithTarget(_myTransform);
        _myTransform.DOKill(true);
        Destroy(gameObject);
    }

    public void WaitInLine(Customer customerToFollow)
    {
        CustomerAnimator.SetBool("Walking", true);
        _followingCustomer = true;
        _customerToFollow = customerToFollow;
        LookTarget = _customerToFollow.transform;
    }

    private void SetMood(CustomerMood mood)
    {
        Mood = mood;

        switch (mood)
        {
            case CustomerMood.Happy:
                SetThoughtSprite(CustomerManager.Singleton.HappySprite);
                break;

            case CustomerMood.Sad:
                SetThoughtSprite(CustomerManager.Singleton.SadSprite);
                break;

            case CustomerMood.Neutral:
                SetThoughtSprite(CustomerManager.Singleton.NeutralSprite);
                break;
        }

        EnableThoughtBubble();
    }

    private void SetThoughtSprite(Sprite sprite)
    {
        ThoughtSprite.sprite = sprite;
    }

    public void ShowCategoryBubble(StockItem.StockCategory category)
    {
        SetThoughtSprite(StockItemManager.Singleton.GetCategorySprite(category));
        EnableThoughtBubble();
    }

    public void ShowItemBubble(string itemName)
    {
        SetThoughtSprite(StockItemManager.Singleton.GetInventorySpriteByItemName(itemName));
        EnableThoughtBubble();
    }

    private void EnableThoughtBubble()
    {
        StopCoroutine(ShowThoughtBubble());
        StartCoroutine(ShowThoughtBubble());
    }

    private IEnumerator ShowThoughtBubble()
    {
        ThoughtBubble.enabled = true;
        ThoughtSprite.enabled = true;
        yield return new WaitForSeconds(2f);
        DisableThoughtBubble();
    }

    private void DisableThoughtBubble()
    {
        ThoughtBubble.enabled = false;
        ThoughtSprite.enabled = false;
    }

    private float GetDistanceToLocation(Vector3 goalLocation)
    {
        return Vector3.Distance(_myTransform.position, goalLocation);
    }
}