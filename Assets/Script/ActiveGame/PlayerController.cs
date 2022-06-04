using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] private float rotateSpeed;
    private PhotonView pv;
    private PlayerInput inputAction;
    private CharacterController controller;
    private Animator animator;
    private Vector2 movementInput;
    private Vector3 currentMovement;
    private Quaternion rotateDir;
    private bool isRun;
    private bool isWalk;

    void Awake()
    {
        pv = gameObject.GetComponentInParent<PhotonView>();
        controller = gameObject.GetComponent<CharacterController>();
        animator  = GetComponent<Animator>();
        inputAction = new PlayerInput();

        inputAction.PlayerController.Movement.started += OnMovementAction;
        inputAction.PlayerController.Movement.performed += OnMovementAction;
        inputAction.PlayerController.Movement.canceled += OnMovementAction;

    }

    private void OnEnable()
    {
        inputAction.PlayerController.Enable();
    }

    private void OnDisable()
    {
        inputAction.PlayerController.Disable();
    }

    private void OnMovementAction(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        currentMovement.x = movementInput.x;
        currentMovement.z = movementInput.y;

        isWalk = movementInput.x != 0 || movementInput.y != 0;
    }

    private void PlayerRotate()
    {
        if (!isWalk) return;
        rotateDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentMovement), Time.deltaTime * rotateSpeed);
        transform.rotation = rotateDir;
    }

    private void AnimationController()
    {
        animator.SetBool("isWalk",isWalk);
    }

    private void Update()
    {
        if (!pv.IsMine) return;
        AnimationController();
        PlayerRotate();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        controller.Move(currentMovement * Time.deltaTime);
    }


}
