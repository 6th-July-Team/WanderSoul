using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button Button_ActivePlayerButton;
    [SerializeField] private Button Button_ActivePetButton;
    [SerializeField] private Button Button_ActiveWagonButton;
    [SerializeField] private Button Button_MoveButton;

    public event Action OnActivePlayerButtonClicked;
    public event Action OnActivePetButtonClicked;
    public event Action OnActiveWagonButtonClicked;
    public event Action OnMoveButtonClicked;

    private ButtonAction _buttonAction;

    private void OnEnable()
    {
        BindButtonEvents();
    }

    private void BindButtonEvents()
    {
        Button_ActivePlayerButton.onClick.AddListener(InvokeActivePlayerButton);
        Button_ActivePetButton.onClick.AddListener(InvokeActivePetButton);
        Button_ActiveWagonButton.onClick.AddListener(InvokeActiveWagonButton);
        Button_MoveButton.onClick.AddListener(InvokeMoveButton);
    }

    private void OnDisable()
    {
        UnBindButtonEvents();
    }

    private void UnBindButtonEvents()
    {
        Button_ActivePlayerButton.onClick.RemoveListener(InvokeActivePlayerButton);
        Button_ActivePetButton.onClick.RemoveListener(InvokeActivePetButton);
        Button_ActiveWagonButton.onClick.RemoveListener(InvokeActiveWagonButton);
        Button_MoveButton.onClick.RemoveListener(InvokeMoveButton);
    }

    private void InvokeActivePlayerButton()
    {
        OnActivePlayerButtonClicked?.Invoke();
    }

    private void InvokeActivePetButton()
    {
        OnActivePetButtonClicked?.Invoke();
    }

    private void InvokeActiveWagonButton()
    {
        OnActiveWagonButtonClicked?.Invoke();
    }

    private void InvokeMoveButton()
    {
        OnMoveButtonClicked?.Invoke();
    }
}

public class ButtonAction
{
    private GameObject _player;
    private GameObject _pet;
    private GameObject _wagon;
    private UI _ui;

    public ButtonAction(UI ui, GameObject player, GameObject wagon, GameObject pet)
    {
        SetField(ui, player, wagon, pet);
        BindEvent();
    }

    private void BindEvent()
    {
        _ui.OnActivePlayerButtonClicked += SetActivePlayer;
        _ui.OnActivePetButtonClicked += SetActivePet;
        _ui.OnActiveWagonButtonClicked += SetActiveWagon;

    }

    private void SetField(UI ui, GameObject player, GameObject wagon, GameObject pet)
    {
        _ui = ui;
        _player = player;
        _pet = pet;
        _wagon = wagon;
    }

    private void SetActivePlayer()
    {
        _player.SetActive(!_player.gameObject.activeSelf);
    }

    private void SetActivePet()
    {
        _pet.SetActive(!_pet.gameObject.activeSelf);
    }

    private void SetActiveWagon()
    {
        _wagon.SetActive(!_wagon.gameObject.activeSelf);
    }
}
