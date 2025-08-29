using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHander : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }

    public PlayerInput PlayerInput;

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;

    //引用背包，后面视情况修改。
    [SerializeField]
    private GameObject inventory;

    public static event System.Action OnInteractPressed;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    public void onMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        
        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;

        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void onJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    public void onInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractPressed?.Invoke();
        }
    }

    public void onOpenInventory(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            inventory.SetActive(!inventory.activeSelf);
        }
    }
}
