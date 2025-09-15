using UnityEngine;

[System.Serializable]
public class SettingsData
{
    private static SettingsData instance;

    public int resolutionWidth;
    public int resolutionHeight;

    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode moveDownKey;
    public KeyCode rotateLeftKey;
    public KeyCode rotateRightKey;
    public KeyCode hardDropKey;

    private SettingsData()
    {
        resolutionWidth = 1920;
        resolutionHeight = 1080;
        moveLeftKey = KeyCode.A;
        moveRightKey = KeyCode.D;
        moveDownKey = KeyCode.S;
        rotateLeftKey = KeyCode.Q;
        rotateRightKey = KeyCode.E;
        hardDropKey = KeyCode.Space;
    }

    public static SettingsData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SettingsData();
            }
            return instance;
        }
    }
}
