using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    Player _player;
    UIManager _uiManager;
    FloatingTextManager _floatingTextManager;
    public Player Player { get { return _player; } }
    public UIManager UIManager { get { return _uiManager; } }
    public FloatingTextManager FloatingTextManager { get { return _floatingTextManager; } }

    private void Awake()
    {
        instance = this;

        // 참조
        _player = FindObjectOfType<Player>();
        _uiManager = FindObjectOfType<UIManager>();
        _floatingTextManager = FindObjectOfType<FloatingTextManager>();

        // 초기화
        _uiManager.InitializeUI(this);

        _player.Initialize(this);
    }
}