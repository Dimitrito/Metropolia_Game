using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject pause;

    [SerializeField] private InputActionAsset actions;

    [SerializeField] private InputActionReference shopActivate;
    [SerializeField] private InputActionReference shopDeactivate;

    [SerializeField] private InputActionReference pauseActivate;
    [SerializeField] private InputActionReference pauseDeactivate;
    private int _BuildMax = 24;
    private int _BuildCount = 0;

    private void OnEnable()
    {
        shopActivate.action.performed += ActivateShop;
        shopDeactivate.action.performed += DeactivateShop;

        pauseActivate.action.performed += ActivatePause;
        pauseDeactivate.action.performed += DeactivatePause;
    }

    private void OnDisable()
    {
        shopActivate.action.performed -= ActivateShop;
        shopDeactivate.action.performed -= DeactivateShop;

        pauseActivate.action.performed -= ActivatePause;
        pauseDeactivate.action.performed -= DeactivatePause;
    }
    private void Awake()
    {
        shopActivate.action.Enable();
        shopDeactivate.action.Enable();

        pauseActivate.action.Enable();
        pauseDeactivate.action.Enable();

        shop.SetActive(false);
    }

    private void ActivateShop(InputAction.CallbackContext context)
    {
        shop.SetActive(true);
        Time.timeScale = 0f;
        actions.FindActionMap("Game").Disable();
        actions.FindActionMap("Shop").Enable();
    }

    private void DeactivateShop(InputAction.CallbackContext context) 
    {
        CloseShop();
    }
    public void CloseShop()
    {
        shop.SetActive(false);
        Time.timeScale = 1.0f;
        actions.FindActionMap("Shop").Disable();
        actions.FindActionMap("Game").Enable();
    }

    private void ActivatePause(InputAction.CallbackContext context)
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
        actions.FindActionMap("Game").Disable();
        actions.FindActionMap("Pause").Enable();
    }

    private void DeactivatePause(InputAction.CallbackContext context)
    {
        ClosePause();
    }

    public void ClosePause()
    {
        pause.SetActive(false);
        Time.timeScale = 1.0f;
        actions.FindActionMap("Pause").Disable();
        actions.FindActionMap("Game").Enable();
    }

    public bool AddBuild() 
    {
        if (_BuildCount < _BuildMax) 
        {
            _BuildCount++;
            return true;
        }
        else
            return false;
    }

    public void RemoveBuild()
    {
        _BuildCount--;
    }
}
