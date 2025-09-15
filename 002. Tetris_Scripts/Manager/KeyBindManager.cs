using TMPro;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    public TMP_InputField moveLeftInput;
    public TMP_InputField moveRightInput;
    public TMP_InputField moveDownInput;
    public TMP_InputField rotateLeftInput;
    public TMP_InputField rotateRightInput;
    public TMP_InputField hardDropInput;

    private TMP_InputField currentInputField;
    private bool isApplying = false;

    void Awake()
    {
        LoadKeyBindings();
        UpdateInputFields();
    }

    void Update()
    {
        if (isApplying) return;

        if (currentInputField != null)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    UpdateKeyBinding(currentInputField, key);
                    break;
                }
            }
        }
    }

    public void ApplyKeyBinds()
    {
        isApplying = true;

        // SettingsData 인스턴스를 통해 데이터 저장
        SettingsData.Instance.moveLeftKey = TryParseKeyCode(moveLeftInput.text, SettingsData.Instance.moveLeftKey);
        SettingsData.Instance.moveRightKey = TryParseKeyCode(moveRightInput.text, SettingsData.Instance.moveRightKey);
        SettingsData.Instance.moveDownKey = TryParseKeyCode(moveDownInput.text, SettingsData.Instance.moveDownKey);
        SettingsData.Instance.rotateLeftKey = TryParseKeyCode(rotateLeftInput.text, SettingsData.Instance.rotateLeftKey);
        SettingsData.Instance.rotateRightKey = TryParseKeyCode(rotateRightInput.text, SettingsData.Instance.rotateRightKey);
        SettingsData.Instance.hardDropKey = TryParseKeyCode(hardDropInput.text, SettingsData.Instance.hardDropKey);

        Debug.Log($"Updated Key Bindings: {SettingsData.Instance.moveLeftKey}, {SettingsData.Instance.moveRightKey}, {SettingsData.Instance.moveDownKey}, {SettingsData.Instance.rotateLeftKey}, {SettingsData.Instance.rotateRightKey}, {SettingsData.Instance.hardDropKey}");

        SaveKeyBindings();
        PlayerPrefs.Save();

        isApplying = false;
    }

    private void SaveKeyBindings()
    {
        PlayerPrefs.SetString("moveLeftKey", SettingsData.Instance.moveLeftKey.ToString());
        PlayerPrefs.SetString("moveRightKey", SettingsData.Instance.moveRightKey.ToString());
        PlayerPrefs.SetString("moveDownKey", SettingsData.Instance.moveDownKey.ToString());
        PlayerPrefs.SetString("rotateLeftKey", SettingsData.Instance.rotateLeftKey.ToString());
        PlayerPrefs.SetString("rotateRightKey", SettingsData.Instance.rotateRightKey.ToString());
        PlayerPrefs.SetString("hardDropKey", SettingsData.Instance.hardDropKey.ToString());

        Debug.Log("Key bindings saved to PlayerPrefs.");
    }

    public void LoadKeyBindings()
    {
        SettingsData.Instance.moveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveLeftKey", SettingsData.Instance.moveLeftKey.ToString()));
        SettingsData.Instance.moveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveRightKey", SettingsData.Instance.moveRightKey.ToString()));
        SettingsData.Instance.moveDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveDownKey", SettingsData.Instance.moveDownKey.ToString()));
        SettingsData.Instance.rotateLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rotateLeftKey", SettingsData.Instance.rotateLeftKey.ToString()));
        SettingsData.Instance.rotateRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rotateRightKey", SettingsData.Instance.rotateRightKey.ToString()));
        SettingsData.Instance.hardDropKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("hardDropKey", SettingsData.Instance.hardDropKey.ToString()));

        UpdateInputFields();
    }

    private void UpdateInputFields()
    {
        moveLeftInput.text = SettingsData.Instance.moveLeftKey.ToString();
        moveRightInput.text = SettingsData.Instance.moveRightKey.ToString();
        moveDownInput.text = SettingsData.Instance.moveDownKey.ToString();
        rotateLeftInput.text = SettingsData.Instance.rotateLeftKey.ToString();
        rotateRightInput.text = SettingsData.Instance.rotateRightKey.ToString();
        hardDropInput.text = SettingsData.Instance.hardDropKey.ToString();
    }

    private KeyCode TryParseKeyCode(string keyString, KeyCode defaultKey)
    {
        string upperKeyString = keyString.ToUpper();

        if (upperKeyString.Equals("SPACE"))
        {
            return KeyCode.Space;
        }

        if (System.Enum.TryParse(upperKeyString, out KeyCode key))
        {
            return key;
        }
        return defaultKey;
    }

    private void UpdateKeyBinding(TMP_InputField inputField, KeyCode key)
    {
        if (inputField == moveLeftInput) SettingsData.Instance.moveLeftKey = key;
        else if (inputField == moveRightInput) SettingsData.Instance.moveRightKey = key;
        else if (inputField == moveDownInput) SettingsData.Instance.moveDownKey = key;
        else if (inputField == rotateLeftInput) SettingsData.Instance.rotateLeftKey = key;
        else if (inputField == rotateRightInput) SettingsData.Instance.rotateRightKey = key;
        else if (inputField == hardDropInput) SettingsData.Instance.hardDropKey = key;

        inputField.text = key.ToString();
    }
}
