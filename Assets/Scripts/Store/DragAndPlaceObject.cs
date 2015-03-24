using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndPlaceObject : MonoBehaviour
{
    public static DragAndPlaceObject Singleton;

    //References

    //Public

    public bool PlacingObject = false;
    public Transform ObjecTransform;
    public float ObjectPrice = 0;

    public FloorTile PreviousTile = null;

    public Material WhiteMaterial;
    public Material RedMaterial;
    public Material GreenMaterial;


    //Private

    void Start()
    {
        Singleton = this;
    }

    void Update()
    {
        //Debug.Log(EventSystem.current.IsPointerOverGameObject() + " " + EventSystem.current.currentSelectedGameObject);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!StoreManager.Singleton.StoreIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlaceShelves();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PlaceFridge();
            }

        }

        if (!PlacingObject || ObjecTransform == null || FloorTile.ActiveTile == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && ObjectPrice > 0)
        {
            CancelPlacement();
            return;
        }

        //if (Input.GetMouseButtonUp(1))
        //{
        //    ObjecTransform.transform.Rotate(new Vector3(0, 90f, 0), Space.World);
        //}

        if (ObjecTransform.position.z == 5f)
        {
            ObjecTransform.transform.rotation = Quaternion.Euler(270, 180, 0);
        }
        else if (ObjecTransform.position.x == 1f)
        {
            ObjecTransform.transform.rotation = Quaternion.Euler(270, 270, 0);
        }
        else if (ObjecTransform.position.z == 1f)
        {
            ObjecTransform.transform.rotation = Quaternion.Euler(270, 0, 0);
        }

        if (!FloorTile.ActiveTile.CanPlaceObjectHere)
        {
            ObjecTransform.GetComponent<Renderer>().material = RedMaterial;
            ObjecTransform.position = FloorTile.ActiveTile.transform.position;
            //Debug.Log("Hovering over other object");
        }
        else
        {
            ObjecTransform.GetComponent<Renderer>().material = GreenMaterial;
            ObjecTransform.position = FloorTile.ActiveTile.transform.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Only if can place
            if (FloorTile.ActiveTile.CanPlaceObjectHere)
            {
                if (ObjectPrice > 0) //New object
                {
                    if (StoreManager.Singleton.Money >= ObjectPrice)
                    {
                        StoreObjectManager.Singleton.StoreObjects.Add(ObjecTransform.gameObject);
                        StoreManager.Singleton.Money -= ObjectPrice;
                        SoundManager.Singleton.PlayCashRegisterSound();
                        UIManager.Singleton.ShowFloatingText("- $" + ObjectPrice.ToString("#0.00"), Color.red, ObjecTransform.position + new Vector3(0, 2, 0));
                        UIManager.Singleton.EnableControlsPanel(false);

                        //First object
                        if (StoreObjectManager.Singleton.StoreObjects.Count == 1)
                        {
                            TutorialManager.Singleton.OnFirstObjectPlaced();
                        }

                        foreach (var tile in StoreManager.Singleton.FloorTiles)
                        {
                            if (tile.CanPlaceObjectHere)
                            {
                                tile.GetComponent<Renderer>().material = tile.WhiteMaterial;
                            }
                        }
                    }
                    else
                    {
                        UIManager.Singleton.ShowFloatingText("Not enough money!", Color.white, ObjecTransform.position + new Vector3(0, 2, 0));
                        return;
                    }
                }

                ObjecTransform.GetComponent<Renderer>().material = WhiteMaterial;

                ObjecTransform.SetParent(FloorTile.ActiveTile.transform, true);
                ObjecTransform.GetComponent<PlaceableObject>().OnPlaced();

                //Placed object on same tile again quickly, show menu
                if (PreviousTile == FloorTile.ActiveTile)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        //UIManager.Singleton.ShowObjectInfoScreen(ObjecTransform);
                    }

                }

                PlacingObject = false;
                ObjecTransform = null;
                PreviousTile = null;
            }
        }
    }

    public void CancelPlacement()
    {
        UIManager.Singleton.EnableControlsPanel(false);
        Destroy(ObjecTransform.gameObject);
        ObjecTransform = null;
        PlacingObject = false;

        foreach (var tile in StoreManager.Singleton.FloorTiles)
        {
            if (tile.CanPlaceObjectHere)
            {
                tile.GetComponent<Renderer>().material = tile.WhiteMaterial;
            }
        }
    }

    public void CreateObjectToPlace(GameObject placeableObjectPrefab)
    {
        if (ObjecTransform != null)
        {
            CancelPlacement();
        }

        PlaceableObject placeableObject = placeableObjectPrefab.GetComponent<PlaceableObject>();

        if (StoreManager.Singleton.Money < placeableObject.Price)
        {
            //Not enough money
            UIManager.Singleton.ShowMessage("Not enough money!", "Not enough money to buy this " + placeableObject.Name + "!");
            return;
        }

        TutorialManager.Singleton.OnStartPlacingObject();
        GameObject newObject = (GameObject)Instantiate(placeableObjectPrefab);
        newObject.name = placeableObject.Name;
        ObjecTransform = newObject.transform;
        PlacingObject = true;
        ObjectPrice = placeableObject.Price;

        UIManager.Singleton.EnableControlsPanel(true);

        foreach (var tile in StoreManager.Singleton.FloorTiles)
        {
            if (tile.CanPlaceObjectHere)
            {
                tile.GetComponent<Renderer>().material = tile.GreenMaterial;
            }
        }
    }

    public void PlaceShelves()
    {
        CreateObjectToPlace(StoreObjectManager.Singleton.ShelvesPrefab);
    }

    public void PlaceFridge()
    {
        CreateObjectToPlace(StoreObjectManager.Singleton.FridgePrefab);
    }
}