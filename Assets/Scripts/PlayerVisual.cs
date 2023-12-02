using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    [SerializeField] private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate() {
        animator.SetBool("isWalking", Player.Instance.IsWalking());
        animator.SetBool("isCrouching", Player.Instance.IsCrouching());
        animator.SetBool("isRunning", Player.Instance.IsRunning());
    }
}
