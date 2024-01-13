using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] TMP_Text nickName;
    PhotonView PV;

    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    private Inputs _input;
    private PlayerInput _playerInput;




    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        _input = GetComponent<Inputs>();

        _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        nickName.text = PV.Owner.NickName;
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        Look();
        Move();
        Jump();
    }

    void Look()
    {
        transform.Rotate(Vector3.up * _input.look.x * mouseSensitivity);

        verticalLookRotation += -_input.look.y * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(_input.move.x, 0, _input.move.y).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (_input.sprint ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (_input.jump && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
            _input.jump = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
            return;

        SetGroundedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == gameObject)
            return;

        SetGroundedState(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == gameObject)
            return;

        SetGroundedState(true);
    }



    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
