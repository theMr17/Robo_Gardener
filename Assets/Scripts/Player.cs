using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player LocalInstance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask collisionsLayerMask;

    private bool isWalking = false;
    private Vector3 lastInteractDir;

    private void Start() {
        PlayerControlsUI.Instance.OnJoyStickMoved += PlayerControlsUI_OnJoyStickMoved;
    }

    void Update() {        
        HandleMovement(GameInput.Instance.GetMovementVectorNormalized());
    }

    private void PlayerControlsUI_OnJoyStickMoved(object sender, PlayerControlsUI.OnJoyStickMovedArgs e) {
        Debug.Log(e);
        HandleMovement(new Vector2(e.horizontal, e.vertical).normalized);
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleMovement(Vector2 inputVector) {
        //Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Debug.Log(inputVector);
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

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
