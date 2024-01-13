using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtbarMove : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (_cam != null)
            transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
