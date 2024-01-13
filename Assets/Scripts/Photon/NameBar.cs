using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBar : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}

