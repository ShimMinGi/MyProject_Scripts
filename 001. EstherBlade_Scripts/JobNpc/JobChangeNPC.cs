using UnityEngine;

public class JobChangeNPC : MonoBehaviour
{
    public GameObject jobUI;

    public void TalkToPlayer()
    {
        Debug.Log("NPC: 새로운 직업을 선택하겠는가?");
        jobUI.SetActive(true);
    }
}