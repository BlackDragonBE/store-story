using UnityEngine;

/// <summary>
/// An object that can be placed into the store
/// </summary>
public class PlaceableObject : MonoBehaviour
{
    //References

    //Public
    public string Name;

    public float Price = 100f;

    [Multiline]
    public string Description;

    public ObjectType PlaceableObjectType;

    public enum ObjectType
    {
        Shelves, Fridge, IceCreamMachine
    }

    //Private
    private Vector3 _previousPosition;

    private void Start()
    {
        //GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        if (GetComponent<ItemStockContainer>().ItemSprite != null && transform.position != _previousPosition)
        {
            GetComponent<ItemStockContainer>().ItemSprite.transform.parent.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
        }

        _previousPosition = transform.position;
    }

    public void OnPlaced()
    {
        //GetComponent<Collider>().enabled = true;

        if (GetComponent<ItemStockContainer>())
        {
            if (GetComponent<ItemStockContainer>().ItemSprite == null)
            {
                UIManager.Singleton.CreateItemSlotForContainer(GetComponent<ItemStockContainer>());
                GetComponent<ItemStockContainer>().ItemSprite.enabled = false;
            }
            else
            {
                GetComponent<ItemStockContainer>().ItemSprite.transform.parent.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
            }
        }

        transform.parent.GetComponent<FloorTile>().CanPlaceObjectHere = false;

        foreach (var tile in StoreManager.Singleton.FloorTiles)
        {
            if (tile.CanPlaceObjectHere)
            {
                tile.GetComponent<Renderer>().material = tile.WhiteMaterial;
            }
        }
    }

    public void OnPickUp()
    {
        //GetComponent<Collider>().enabled = false;
        transform.parent.GetComponent<FloorTile>().CanPlaceObjectHere = true;

        foreach (var tile in StoreManager.Singleton.FloorTiles)
        {
            if (tile.CanPlaceObjectHere)
            {
                tile.GetComponent<Renderer>().material = tile.GreenMaterial;
            }
        }
    }

    //public void OnMouseDown()
    //{
    //    if (transform.parent != null)
    //    {
    //        transform.parent.GetComponent<FloorTile>().OnMouseDown();
    //    }
    //}

    //public void OnMouseEnter()
    //{
    //    FloorTile.ActiveTile = transform.parent.GetComponent<FloorTile>();
    //}

    
}