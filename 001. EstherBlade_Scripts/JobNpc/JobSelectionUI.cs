using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject jobButtonPrefab;
    [SerializeField] private Transform jobButtonContainer;

    private PlayerStatus playerStatus;
    private JobInfoLoader loader;
    private PlayerInput playerInput;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void InitializeUI(GameManager gameManager)
    {
        playerStatus = gameManager.Player.GetComponent<PlayerStatus>();
        playerInput = gameManager.Player.GetComponent<PlayerInput>();

        loader = new JobInfoLoader();
        CreateJobButtons();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        if (playerInput != null)
            playerInput.TogglePlayerControl(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (playerInput != null)
            playerInput.TogglePlayerControl(true);
    }

    private void CreateJobButtons()
    {
        foreach (var job in loader.ItemsList)
        {
            var btnObj = Instantiate(jobButtonPrefab, jobButtonContainer);
            btnObj.GetComponentInChildren<TMP_Text>().text = job.jobName;

            var j = job;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                playerStatus.ChangeJob(j);
                Close(); // 직업 선택 후 UI 닫기
            });
        }
    }
}