﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{


    [SerializeField] bool isAiming = true;
    [SerializeField] Transform target;


    CharacterController2D characterController;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        characterController = transform.root.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAiming) return;

        target.position = camera.ScreenToWorldPoint(Input.mousePosition);
        
        var pos = camera.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        
        
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90 ;
        if (characterController.m_FacingRight) angle -= 10;
        else angle += 10;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
