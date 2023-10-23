using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public Sprite emptyHeart, halfHeart, fullHeart;
    Image image;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public void SetHeartImage(HeartStatus status) {
        switch (status) {
            case HeartStatus.Empty:
                image.sprite = emptyHeart;
                break;
            case HeartStatus.Half:
                image.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                image.sprite = fullHeart;
                break;
        }
    } 
}

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}
