using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset inputActions; // The Input Actions asset

    [Header("Jump")]
    public TMP_Text jumpbindingDisplayText;   // Text to display the current binding
    public Button jumprebindButton;          // Button to trigger rebinding
    public TMP_Text jumprebindPromptText;    // Prompt text for rebinding
    public TMP_Text errorPromptText;

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
    }

    private void StartRebindingJump()
    {
        if (jump == null)
            return;

        jumprebindPromptText.text = "Press any key...";
        jumpbindingDisplayText.gameObject.SetActive(false);
        jumprebindPromptText.gameObject.SetActive(true);

        jump.Disable();

        jump.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnPotentialMatch((context) =>
            {
                var newPath = context.candidates[bindingIndex].path;
                Debug.Log(context.candidates[bindingIndex].path);
                Debug.Log(newPath);
                if (IsKeyConflict(newPath))
                {
                    Debug.Log("Conflito");
                    // Conflict detected
                    context.Cancel();
                    errorPromptText.text = $"Key '{InputControlPath.ToHumanReadableString(newPath)}' is already in use!";
                    errorPromptText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Passou");
                    errorPromptText.gameObject.SetActive(false);
                }
            })
            .OnComplete(operation =>
            {
                Debug.Log("Entramos no complete");
                jump.Enable();
                operation.Dispose();
                jumprebindPromptText.gameObject.SetActive(false);
                jumpbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
                
            })
            .OnCancel(operation =>
            {
                Debug.Log("OnCancel");
                operation.Dispose();
                jumprebindPromptText.gameObject.SetActive(false);
                jumpbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
            })
            .Start();
    }

    private void StartRebindingDash()
    {
        if (dash == null)
            return;

        dashrebindPromptText.text = "Press any key...";
        dashbindingDisplayText.gameObject.SetActive(false);
        dashrebindPromptText.gameObject.SetActive(true);

        dash.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                operation.Dispose();
                dashrebindPromptText.gameObject.SetActive(false);
                dashbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
            })
            .Start();
    }

    private void StartRebindingParry()
    {
        if (parry == null)
            return;

        parryrebindPromptText.text = "Press any key...";
        parrybindingDisplayText.gameObject.SetActive(false);
        parryrebindPromptText.gameObject.SetActive(true);

        parry.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                operation.Dispose();
                parryrebindPromptText.gameObject.SetActive(false);
                parrybindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
            })
            .Start();
    }
    private void StartRebindingAttack()
    {
        if (attack == null)
            return;

        attackrebindPromptText.text = "Press any key...";
        attackbindingDisplayText.gameObject.SetActive(false);
        attackrebindPromptText.gameObject.SetActive(true);

        attack.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                operation.Dispose();
                attackrebindPromptText.gameObject.SetActive(false);
                attackbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
            })
            .Start();
    }
    private void StartRebindingInteract()
    {
        if (interact == null)
            return;

        interactrebindPromptText.text = "Press any key...";
        interactbindingDisplayText.gameObject.SetActive(false);
        interactrebindPromptText.gameObject.SetActive(true);

        interact.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                operation.Dispose();
                interactrebindPromptText.gameObject.SetActive(false);
                interactbindingDisplayText.gameObject.SetActive(true);
                UpdateBindingDisplay();
            })
            .Start();
    }
    private bool IsKeyConflict(string newPath)
    {
        foreach (var action in inputActions)
        {
            foreach (var binding in action.bindings)
            {
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