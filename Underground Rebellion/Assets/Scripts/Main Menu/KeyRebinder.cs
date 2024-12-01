using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class KeyRebinder : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset inputActions; // The Input Actions asset
    public TMP_Text errorPrompt;

    [Header("Jump")]
    public TMP_Text jumpbindingDisplayText;   // Text to display the current binding
    public Button jumprebindButton;          // Button to trigger rebinding
    public TMP_Text jumprebindPromptText;    // Prompt text for rebinding

    [Header("Dash")]
    public TMP_Text dashbindingDisplayText;   // Text to display the current binding
    public Button dashrebindButton;          // Button to trigger rebinding
    public TMP_Text dashrebindPromptText;    // Prompt text for rebinding

    [Header("Parry")]
    public TMP_Text parrybindingDisplayText;   // Text to display the current binding
    public Button parryrebindButton;          // Button to trigger rebinding
    public TMP_Text parryrebindPromptText;    // Prompt text for rebinding

    [Header("Attack")]
    public TMP_Text attackbindingDisplayText;   // Text to display the current binding
    public Button attackrebindButton;          // Button to trigger rebinding
    public TMP_Text attackrebindPromptText;    // Prompt text for rebinding

    [Header("Interact")]
    public TMP_Text interactbindingDisplayText;   // Text to display the current binding
    public Button interactrebindButton;          // Button to trigger rebinding
    public TMP_Text interactrebindPromptText;    // Prompt text for rebinding

    private InputAction jump, dash, parry, attack, interact;
    private int bindingIndex;
    private const string RebindingOverridesKey = "RebindingOverrides";

    private bool rebinding = false;

    private void Start()
    {
        // Example: Set up the Jump action and its binding display
        jump = inputActions.FindAction("Jump"); // Replace with your action name
        dash = inputActions.FindAction("Dash"); // Replace with your action name
        parry = inputActions.FindAction("Parry"); // Replace with your action name
        attack = inputActions.FindAction("Attack"); // Replace with your action name
        interact = inputActions.FindAction("Interact"); // Replace with your action name
        bindingIndex = 0; // Typically 0 unless you have multiple bindings for the same action
        UpdateBindingDisplay();

        jumprebindButton.onClick.AddListener(StartRebindingJump);
        dashrebindButton.onClick.AddListener(StartRebindingDash);
        parryrebindButton.onClick.AddListener(StartRebindingParry);
        attackrebindButton.onClick.AddListener(StartRebindingAttack);
        interactrebindButton.onClick.AddListener(StartRebindingInteract);
    }

    private void UpdateBindingDisplay()
    {
        if (jump != null)
        {
            string bindingName = jump.bindings[bindingIndex].ToDisplayString();
            jumpbindingDisplayText.text = $"Jump Key: {bindingName}";
        }
        if (dash != null)
        {
            string bindingName = dash.bindings[bindingIndex].ToDisplayString();
            dashbindingDisplayText.text = $"Dash Key: {bindingName}";
        }
        if (parry != null)
        {
            string bindingName = parry.bindings[bindingIndex].ToDisplayString();
            parrybindingDisplayText.text = $"Parry Key: {bindingName}";
        }
        if (attack != null)
        {
            string bindingName = attack.bindings[bindingIndex].ToDisplayString();
            attackbindingDisplayText.text = $"Attack Key: {bindingName}";
        }
        if (interact != null)
        {
            string bindingName = interact.bindings[bindingIndex].ToDisplayString();
            interactbindingDisplayText.text = $"Interact Key: {bindingName}";
        }

        EventSystem.current.SetSelectedGameObject(null);

	}

    private void StartRebindingJump()
    {
        if (jump == null && rebinding)
            return;

        rebinding = true;
        jumprebindPromptText.text = "Press any key...";
        jumpbindingDisplayText.gameObject.SetActive(false);
        errorPrompt.gameObject.SetActive(false);
        jumprebindPromptText.gameObject.SetActive(true);

        jump.Disable();

        jump.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
			.WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                if (IsKeyConflict(newPath))
                {
                    // Conflict detected
                    context.Cancel();
					errorPrompt.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
					errorPrompt.gameObject.SetActive(true);
                    jumprebindPromptText.gameObject.SetActive(false);
                    jumpbindingDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    errorPrompt.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                jump.Enable();
                operation.Dispose();
                jumprebindPromptText.gameObject.SetActive(false);
                jumpbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
                rebinding = false;
                
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                jumprebindPromptText.gameObject.SetActive(false);
                jumpbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .Start();
    }

    private void StartRebindingDash()
    {
        if (dash == null && rebinding)
            return;

		rebinding = true;

		dashrebindPromptText.text = "Press any key...";
        dashbindingDisplayText.gameObject.SetActive(false);
		errorPrompt.gameObject.SetActive(false);
        dashrebindPromptText.gameObject.SetActive(true);

        dash.Disable();

        dash.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                if (IsKeyConflict(newPath))
                {
                    // Conflict detected
                    context.Cancel();
                    errorPrompt.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
                    errorPrompt.gameObject.SetActive(true);
                    dashrebindPromptText.gameObject.SetActive(false);
                    dashbindingDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    errorPrompt.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                dash.Enable();
                operation.Dispose();
                dashrebindPromptText.gameObject.SetActive(false);
                dashbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;

			})
            .OnCancel(operation =>
            {
                operation.Dispose();
                dashrebindPromptText.gameObject.SetActive(false);
                dashbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .Start();
    }

    private void StartRebindingParry()
    {
        if (parry == null && rebinding)
            return;

        rebinding = true;

        parryrebindPromptText.text = "Press any key...";
        parrybindingDisplayText.gameObject.SetActive(false);
		errorPrompt.gameObject.SetActive(false);
        parryrebindPromptText.gameObject.SetActive(true);

        parry.Disable();

        parry.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                if (IsKeyConflict(newPath))
                {
                    // Conflict detected
                    context.Cancel();
                    errorPrompt.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
                    errorPrompt.gameObject.SetActive(true);
                    parryrebindPromptText.gameObject.SetActive(false);
                    parrybindingDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    errorPrompt.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                parry.Enable();
                operation.Dispose();
                parryrebindPromptText.gameObject.SetActive(false);
                parrybindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;

			})
            .OnCancel(operation =>
            {
                operation.Dispose();
                parryrebindPromptText.gameObject.SetActive(false);
                parrybindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .Start();
    }
    private void StartRebindingAttack()
    {
        if (attack == null && rebinding)
            return;

        rebinding = true;

        attackrebindPromptText.text = "Press any key...";
        attackbindingDisplayText.gameObject.SetActive(false);
		errorPrompt.gameObject.SetActive(false);
        attackrebindPromptText.gameObject.SetActive(true);

        attack.Disable();

        attack.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                if (IsKeyConflict(newPath))
                {
                    // Conflict detected
                    context.Cancel();
                    errorPrompt.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
                    errorPrompt.gameObject.SetActive(true);
                    attackrebindPromptText.gameObject.SetActive(false);
                    attackbindingDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    errorPrompt.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                attack.Enable();
                operation.Dispose();
                attackrebindPromptText.gameObject.SetActive(false);
                attackbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .OnCancel(operation =>
            {
                operation.Dispose();
                attackrebindPromptText.gameObject.SetActive(false);
                attackbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .Start();
    }
    private void StartRebindingInteract()
    {
        if (interact == null && rebinding)
            return;

        rebinding = true;

        interactrebindPromptText.text = "Press any key...";
        interactbindingDisplayText.gameObject.SetActive(false);
		errorPrompt.gameObject.SetActive(false);
        interactrebindPromptText.gameObject.SetActive(true);

        interact.Disable();

        interact.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
			.WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                if (IsKeyConflict(newPath))
                {
                    // Conflict detected
                    context.Cancel();
                    errorPrompt.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
                    errorPrompt.gameObject.SetActive(true);
                    interactrebindPromptText.gameObject.SetActive(false);
                    interactbindingDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    errorPrompt.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                interact.Enable();
                operation.Dispose();
                interactrebindPromptText.gameObject.SetActive(false);
                interactbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;

			})
            .OnCancel(operation =>
            {
                operation.Dispose();
                interactrebindPromptText.gameObject.SetActive(false);
                interactbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
				rebinding = false;
			})
            .Start();
    }
    private bool IsKeyConflict(string newPath)
    {
        string[] splitString = newPath.Split('/');
        newPath = "<" + splitString[1] + ">/" + splitString[2] ;
        foreach (var action in inputActions)
        {
            foreach (var binding in action.bindings)
            {
                Debug.Log("Bindings found: " + binding.effectivePath);
                if (binding.effectivePath == newPath)
                {
                    return true; // Key is already in use
                }
            }
        }
        return false; // Key is available
    }

    private void SaveBindingOverrides()
    {
        string overrides = inputActions.ToJson();
        PlayerPrefs.SetString(RebindingOverridesKey, overrides);
        PlayerPrefs.Save();
        Debug.Log("Binding overrides saved.");
    }

    private void LoadBindingOverrides()
    {
        if (PlayerPrefs.HasKey(RebindingOverridesKey))
        {
            string overrides = PlayerPrefs.GetString(RebindingOverridesKey);
            inputActions.LoadFromJson(overrides);
            Debug.Log("Binding overrides loaded.");
        }
    }
}