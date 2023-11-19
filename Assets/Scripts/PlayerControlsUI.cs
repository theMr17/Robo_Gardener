using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsUI : MonoBehaviour {
    public static PlayerControlsUI Instance {get; private set;}

    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _interactButton;

    public event EventHandler<OnJoyStickMovedArgs> OnJoyStickMoved;
    public class OnJoyStickMovedArgs : EventArgs {
        public float horizontal;
        public float vertical;
    }
    public event EventHandler OnInteract;

    private void Awake() {
        Instance = this;

        // _interactButton.onClick.AddListener(() => {
        //     OnInteract?.Invoke(this, EventArgs.Empty);
        // });

        //PlayerCanInteract(false);
    }

    private void Update() {
        OnJoyStickMoved?.Invoke(this, new OnJoyStickMovedArgs {
            horizontal = _joystick.Horizontal,
            vertical = _joystick.Vertical
        });
    }

    public void PlayerCanInteract(bool canInteract) {
        _interactButton.interactable = canInteract;
    }
}
