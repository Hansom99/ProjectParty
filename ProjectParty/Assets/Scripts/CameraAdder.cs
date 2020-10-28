using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdder : MonoBehaviour
{

    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
