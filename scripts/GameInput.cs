using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAfternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;
    private PlayerInputAction inputActions;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Afternate,
        Pause
    }

    void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputAction();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAfternate.performed += InteractAfternate_performed;
        inputActions.Player.Pause.performed += Pause;

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAfternate.performed -= InteractAfternate_performed;
        inputActions.Player.Pause.performed -= Pause;

        inputActions.Dispose();
    }

    private void Pause(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    private void InteractAfternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAfternateAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputDiraction = inputActions.Player.Move.ReadValue<Vector2>();
        return inputDiraction.normalized;
    }

    public string GetBingdingText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Move_Up:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Afternate:
                return inputActions.Player.InteractAfternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action action)
    {
        inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = inputActions.Player.Move;
                bindingIndex = 1;
            break;
            case Binding.Move_Down:
                inputAction = inputActions.Player.Move;
                bindingIndex = 2;
            break;
            case Binding.Move_Left:
                inputAction = inputActions.Player.Move;
                bindingIndex = 3;
            break;
            case Binding.Move_Right:
                inputAction = inputActions.Player.Move;
                bindingIndex = 4;
            break;
            case Binding.Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 0;
            break;
            case Binding.Interact_Afternate:
                inputAction = inputActions.Player.InteractAfternate;
                bindingIndex = 0;
            break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 0;
            break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete( callback =>
        {
            callback.Dispose();
            inputActions.Player.Enable();
            action();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS,inputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this,EventArgs.Empty);
        }).Start();
    }
}
