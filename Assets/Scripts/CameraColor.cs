using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    
    public void ChangeColor(bool isNight)
    {
        GetComponent<Camera>().backgroundColor = isNight ? nightColor : dayColor; 
    }
}
