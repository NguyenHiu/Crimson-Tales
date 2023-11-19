using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Item item;
    Image image;
    Transform parentTransform;

    void Awake()
    {
        GetImageComponent();
    }

    void GetImageComponent()
    {
        image = GetComponent<Image>();
    }

    public Item Item { get { return item; } }

    public void Init(Item newItem)
    {
        item = newItem;
        if (!image) GetImageComponent();
        image.sprite = item.Sprite;
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
