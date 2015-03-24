using UnityEngine;
using UnityEngine.EventSystems;

public class FloorTile : MonoBehaviour
{
    //References

    //Public
    public static FloorTile ActiveTile;

    public bool CanPlaceObjectHere = true;

    public Material WhiteMaterial;
    public Material RedMaterial;
    public Material GreenMaterial;

    //Private

    private void Start()
    {
    }

    private void Update()
    {
        //name = "FloorTile " + transform.position.x + " " + transform.position.z;
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (transform.childCount > 0 && !DragAndPlaceObject.Singleton.PlacingObject)
        {
            DragAndPlaceObject.Singleton.ObjecTransform = transform.GetChild(0);
            DragAndPlaceObject.Singleton.PlacingObject = true;
            DragAndPlaceObject.Singleton.ObjectPrice = 0;
            DragAndPlaceObject.Singleton.PreviousTile = this;
            transform.GetChild(0).GetComponent<PlaceableObject>().OnPickUp();
            transform.GetChild(0).SetParent(null, true);
        }
    }

    //public void OnMouseOver()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject())
    //    {
    //        return;
    //    }

    //    if (Input.GetMouseButtonUp(1))
    //    {
    //        if (transform.childCount > 0 && DragAndPlaceObject.Singleton.PlacingObject == false)
    //        {
    //            //Show object screen
    //            UIManager.Singleton.ShowObjectInfoScreen(transform.GetChild(0));
    //        }
    //    }
    //}

    public void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        ActiveTile = this;

        //if (DragAndPlaceObject.Singleton.PlacingObject)
        //{
        //    if (CanPlaceObjectHere)
        //    {
        //        GetComponent<Renderer>().material = GreenMaterial;
        //    }
        //    else
        //    {
        //        GetComponent<Renderer>().material = RedMaterial;
        //    }
        //}


    }

    //public void OnMouseExit()
    //{
    //    GetComponent<Renderer>().material = WhiteMaterial;
    //}
}