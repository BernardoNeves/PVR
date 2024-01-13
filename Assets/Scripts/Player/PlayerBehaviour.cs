using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviour
{

    [SerializeField] private Transform PlayerCamera;
    public Inputs _input;
    public static Action shootInput;
    public static Action reloadInput;
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine) Destroy(this);

    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if (_input.shoot)
            shootInput?.Invoke();

        if (_input.reload)
            reloadInput?.Invoke();
    }
    

    public void Interact() {

        Ray ray = new (PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 3))
        {
            InteractableInterface interactableInterface = hitInfo.transform.GetComponent<InteractableInterface>();
            interactableInterface?.Interact();
        }
    }
}
