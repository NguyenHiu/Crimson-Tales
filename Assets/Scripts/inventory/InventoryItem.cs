using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;
    private Transform parentTransform;

    public void Init(Item newItem)
    {
        item = newItem;
        image.sprite = item.sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector2)pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransform);
        image.raycastTarget = true;
    }

    public void SetNewParentTransform(Transform newParent)
    {
        parentTransform = newParent;
    }
}
