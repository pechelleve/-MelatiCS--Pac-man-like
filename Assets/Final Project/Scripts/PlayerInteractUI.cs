using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactUIGameObject;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        if (playerInteraction.GetInteractibleObject() != null)
        {
            Show(playerInteraction.GetInteractibleObject());
        }
        else 
        {
            Hide();
        }
    }

    private void Show(Interactable interactable)
    {
        interactUIGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        interactUIGameObject.SetActive(false);
    }
}
