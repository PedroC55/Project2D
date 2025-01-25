using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public InputActionAsset inputActions; // The Input Actions asset
    private static int nParries;

    private InputAction jump, parry, attack, movement;
    private int bindingIndex;
    private int movement_leftIndex, movement_rightIndex;

    public GameObject enemy;

    //Tutorial TextBox
    [SerializeField] private TextMeshProUGUI movement_tutorialText;
    [SerializeField] private TextMeshProUGUI jump_tutorialText;
    [SerializeField] private TextMeshProUGUI sneak_tutorialText;
    [SerializeField] private TextMeshProUGUI parry2_tutorialText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        jump = inputActions.FindAction("Jump"); // Replace with your action name
        parry = inputActions.FindAction("Parry"); // Replace with your action name
        attack = inputActions.FindAction("Attack"); // Replace with your action name
        movement = inputActions.FindAction("Movement");
        bindingIndex = 0; // Typically 0 unless you have multiple bindings for the same action
        movement_leftIndex = 1;
        movement_rightIndex = 2;
        movement_tutorialText.text = string.Format("Hold '{0}' to walk to the left\n\nHold '{1}' to walk to the right", movement.bindings[movement_leftIndex].ToDisplayString(), movement.bindings[movement_rightIndex].ToDisplayString());
        jump_tutorialText.text = string.Format("'{0}' to jump", jump.bindings[bindingIndex].ToDisplayString());
		sneak_tutorialText.text = string.Format("Attack the enemies from behind to defeat them.\n\nPress '{0}' to attack.", attack.bindings[bindingIndex].ToDisplayString());
        parry2_tutorialText.text = string.Format("Press '{0}' to parry the enemy at the right time, to lower their energy\n\nProgress: 0 / 2 Parries", parry.bindings[bindingIndex].ToDisplayString());

    }

    private void OnEnable()
    {
        SettingsEvent.OnRebind += UpdateTutorialText;
    }
    private void OnDisable()
    {
        SettingsEvent.OnRebind -= UpdateTutorialText;
    }

    private void Update()
    {
        if (enemy != null && !enemy.activeSelf)
        {
            ShowMapTutorial();
        }
    }


    public void UpdateParryProgress(int parryTimes)
    {
        nParries = parryTimes;
        if (parryTimes < 2)
        {
            parry2_tutorialText.text = string.Format("Press '{0}' to parry the enemy at the right time, to lower their energy\n\nProgress: {1} / 2 Parries", parry.bindings[bindingIndex].ToDisplayString(), nParries);
        }else if (parryTimes == 2)
        {
            parry2_tutorialText.text = string.Format("Press '{0}' to parry the enemy at the right time, to lower their energy\n\nProgress: {1} / 2 Parries", parry.bindings[bindingIndex].ToDisplayString(), nParries);
            ShowAttackBox();
        }
        
    }
    
    private void ShowAttackBox()
    {
        parry2_tutorialText.text = string.Format("Press 'K' to attack the enemy");
    }

    private void ShowMapTutorial()
    {
        parry2_tutorialText.text = string.Format("Press 'M' to show the map\n\nLet the adventure begin!");
    }

    private void UpdateTutorialText(InputAction action)
    {
        movement_tutorialText.text = string.Format("Hold '{0}' to walk to the left\n\nHold '{1}' to walk to the right", movement.bindings[movement_leftIndex].ToDisplayString(), movement.bindings[movement_rightIndex].ToDisplayString());
        jump_tutorialText.text = string.Format("'{0}' to jump", jump.bindings[bindingIndex].ToDisplayString());
		sneak_tutorialText.text = string.Format("Attack the enemies from behind to defeat them.\n\nPress '{0}' to attack.", attack.bindings[bindingIndex].ToDisplayString());
		UpdateParryProgress(nParries);
    }
}
