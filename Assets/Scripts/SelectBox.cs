using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    [SerializeField]
    private string next_Event; // 다음 대사

    public void SetEventName(string _eventName)
    {
        next_Event = _eventName;
    }

    public void Next_Dialouge()
    {
        Dialogue_Manage.instance.GetEventName(next_Event.ToString());

        for(int i = 0; i < this.transform.parent.childCount; i++)
        {
            //모든 선택창 감추기
            this.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
