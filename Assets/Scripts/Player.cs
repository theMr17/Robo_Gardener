using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float runSpeed = 18f;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float crouchWalkSpeed = 5.5f;
    [SerializeField] private LayerMask collisionsLayerMask;

    private bool isWalking = false;
    private bool isCrouching = false;
    private bool isRunning = false;
    private Vector3 lastInteractDir;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        PlayerControlsUI.Instance.OnJoyStickMoved += PlayerControlsUI_OnJoyStickMoved;
        GameInput.Instance.OnCrouchAction += GameInput_OnCrouchAction;
        GameInput.Instance.OnRunAction += GameInput_OnRunAction;
    }

    private void GameInput_OnCrouchAction(object sender, EventArgs e) {
        isCrouching = !isCrouching;
    }

    private void GameInput_OnRunAction(object sender, EventArgs e) {
        isRunning = !isRunning;
    }


    void Update() {        
        HandleMovement(GameInput.Instance.GetMovementVectorNormalized());
    }

    private void PlayerControlsUI_OnJoyStickMoved(object sender, PlayerControlsUI.OnJoyStickMovedArgs e) {
        // Debug.Log(e);
        HandleMovement(new Vector2(e.horizontal, e.vertical).normalized);
    }

    public bool IsWalking() {
        return isWalking;
    }

    public bool IsCrouching() {
        return isCrouching;
    }

    public bool IsRunning() {
        return isRunning && isWalking;
    }

    private void HandleMovement(Vector2 inputVector) {
        Debug.Log(inputVector);
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveSpeed = isCrouching ? crouchWalkSpeed : isRunning ? runSpeed : walkSpeed;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);

        if(!canMove) {
            // cannot move towards moveDir
            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);

            if(canMove) {
                // can move only on the X
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);

                if(canMove) {
                    // can move only on the Z
                    moveDir = moveDirZ;
                } else {
                    // Cannot move in any direction
                } 
            } 
            
        }
        
        if(canMove) {
            transform.position += moveDir * moveDistance;   
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }
}
