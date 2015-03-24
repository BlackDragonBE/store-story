using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Singleton;

    //References

    //Public
    public List<Customer> Customers = new List<Customer>();

    public int HappyCustomers;
    public int NeutralCustomers;
    public int SadCustomers;

    //Prefabs
    [Header("Prefabs")]
    public GameObject CustomerPrefab;

    [Header("Customer Sprites")]
    public Sprite HappySprite;

    public Sprite SadSprite;
    public Sprite NeutralSprite;

    //Private
    private bool _spawingCustomers;

    public List<Customer> _customersReadyToPay = new List<Customer>();

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCustomer();
        }
    }

    public void StartSpawningCustomers()
    {
        _spawingCustomers = true;
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (_spawingCustomers && StoreManager.Singleton.StoreIsOpen)
        {
            yield return new WaitForSeconds(Random.Range(2f - (StoreManager.Singleton.StorePopularity * 0.015f), 5f - (StoreManager.Singleton.StorePopularity * 0.04f))); //TODO: time between new customer spawn, should depend on different factors

            if (_spawingCustomers)
            {
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        Instantiate(CustomerPrefab, new Vector3(-7f, 0, -5f), Quaternion.identity);
    }

    public void StopSpawningCustomers()
    {
        //Debug.Log("Stop Spawning Customers");
        _spawingCustomers = false;
    }

    public void ReadyToPay(Customer customer)
    {
        _customersReadyToPay.Add(customer);

        if (_customersReadyToPay.Count == 1)
        {
            customer.StartCoroutine(customer.GoToRegisterAndExit());
        }
        else
        {
            customer.WaitInLine(_customersReadyToPay[_customersReadyToPay.Count - 2]);
        }
        //TODO: decide if customer can go to register or has to wait in line
    }

    public void OnCustomerLeft(Customer customer, bool purchasedItems)
    {
        Customers.Remove(customer);
        _customersReadyToPay.Remove(customer);

        switch (customer.Mood)
        {
            case Customer.CustomerMood.Happy:
                HappyCustomers++;
                break;

            case Customer.CustomerMood.Sad:
                SadCustomers++;
                break;

            case Customer.CustomerMood.Neutral:
                NeutralCustomers++;
                break;
        }

        if (purchasedItems && _customersReadyToPay.Count > 0)
        {
            //Debug.Log("customer 0: go to register");
            //If there's a customer waiting in line, tell it it can go to the register
            _customersReadyToPay[0].StartCoroutine(
                _customersReadyToPay[0].GoToRegisterAndExit());
        }
    }
}