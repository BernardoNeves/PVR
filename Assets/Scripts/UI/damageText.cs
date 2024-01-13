using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageText : MonoBehaviour
{
    public float destroyTime = 1.5f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += new Vector3(Random.Range(-1f, 1f), 0, 0);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
