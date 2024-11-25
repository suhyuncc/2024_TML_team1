using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public string[] dialogue_Context; // 대화 내용
    public int Left_id; // 현재 대화중인 대상 ... 현재 딱히 필요없어보임 아마, 오브젝트 
    public int Right_id; // 현재 대화중인 대상 ... 현재 딱히 필요없어보임 아마, 오브젝트 
    public int Speaker; // 현재 화자
    public string[] is_select; // 선택지가 있는가?
    public string[] Secletion_Context; // 선택지 내용
    public string[] Next_event; // 선택지 다음 event_name
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //현재 진행중인 대화 종류

    [SerializeField] string[] dialogue_Data;
}
