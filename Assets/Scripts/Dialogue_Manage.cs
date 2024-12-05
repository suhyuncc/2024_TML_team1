using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//gh
public class Dialogue_Manage : MonoBehaviour
{
    public static Dialogue_Manage instance;

    [SerializeField]
    private PlayerInfo _playerInfo;
    public string eventName; // eventName 수령 받을 곳
    [SerializeField]
    private TMP_Text contextText; //대화
    [SerializeField]
    private GameObject endTriangle; //끝나면 깜빡거리는 삼각형

    private bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working

    private DialogueData[] dialogueData; //대화 데이터

    [SerializeField]
    private GameObject _credit;
    [SerializeField]
    private GameObject dialoguePanel; //대화panel
    [SerializeField]
    private Image _rightImage;
    [SerializeField]
    private Image _leftImage;
    [SerializeField]
    private Sprite[] _chitoImages;
    [SerializeField]
    private GameObject SelectBoxes; //선택지 박스들

    [SerializeField]
    private int isStageNumber = 0;

    private string eventNameIf2Event = null;

    public void ItIsPreviousDialogue(int num)
    {
        isStageNumber = num;
    }

    public void ItisDoubleDialogue(string event1, string event2)
    {
        GetEventName(event1);
        eventNameIf2Event = event2;
    }

    public void ItIsBothDialogue(string _eventName)
    {
        eventNameIf2Event = _eventName;
    }

    public void GetEventName(string _eventName) //eventName 수령받는 함수
    {
        eventName = _eventName;
        isDialogue = true;
        if(dialoguePanel.activeSelf == false)
            dialoguePanel.SetActive(true);
    }
    
    private int dataIndex = 0;
    private int contextIndex = 0;

    private bool currentTypeEnd = false;
    private string toType = null;

    private IEnumerator typingText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

        if(isDialogue)
        {

            isDialogue = false;
            currentDialogue= true;
            dialogueData = CSVParsingD.GetDialogue(eventName); // 화자 타입, 화자 이름, 대사를 원하는 이벤트에 있는 내용을 가져옴
            endTriangle.SetActive(false);

            dataIndex = 0;
            contextIndex = 0;
            currentTypeEnd = false;
            typingText = TypingMotion();
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];

            //Left_imageCheck(dataIndex);
            Right_imageCheck(dataIndex);

            StartCoroutine(typingText);
        }//EventName을 받아왔을 때

        //다이알로그 진행중이고 클릭이 되면
        if (currentDialogue && Input.GetMouseButtonDown(0))
        {
            //코루틴이 끝났는지
            if (currentTypeEnd)
            {
                //지금 출력중인 문장이 다 출력 안되어있으면
                if (contextIndex < dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    contextIndex++;
                    currentTypeEnd = false;
                    typingText = TypingMotion();
                    endTriangle.SetActive(false);
                    toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                    StartCoroutine(typingText);
                }
                //지금 출력중인 문장이 다 출력 되어있으면
                else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    contextIndex = 0;
                    dataIndex++;
                    if (dataIndex < dialogueData.Length)
                    {
                        currentTypeEnd = false;
                        typingText = TypingMotion();
                        endTriangle.SetActive(false);
                        toType = dialogueData[dataIndex].dialogue_Context[contextIndex];

                        //Left_imageCheck(dataIndex);
                        Right_imageCheck(dataIndex);

                        StartCoroutine(typingText); ;
                    }
                    else
                    {
                        currentDialogue = false;
                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //선택지 보여주기
                            selectbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "2")
                        {
                            contextIndex = 0;
                            //2번째 선택지 창 열기
                            selectbox2();
                        }
                        else 
                        {
                            dialoguePanel.SetActive(false);

                            if (_playerInfo.current_stage == 11)
                            {
                                _credit.SetActive(true);
                            }
                        }
                        
                    }
                }
            }
            else
            {
                StopCoroutine(typingText);
                contextText.text = toType;
                currentTypeEnd = true;
                endTriangle.SetActive(true);
            }
        }
    }
    IEnumerator TypingMotion()
    {
        contextText.text = null;
        for(int i = 0; i < toType.Length; i++)
        {
            contextText.text+= toType[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        currentTypeEnd = true;
        endTriangle.SetActive(true);
        yield break;
    }

    private void Left_imageCheck(int index)
    {
        //왼쪽 이미지를 켜야한다면 그 아이디에 맞춰 켜기
        if (dialogueData[index].Left_id != 0)
        {
            _leftImage.sprite = _chitoImages[dialogueData[index].Left_id - 1];
            _leftImage.SetNativeSize();
            _leftImage.gameObject.SetActive(true);

            //화자인지 아닌지 체크
            if(dialogueData[index].Left_id != dialogueData[index].Speaker)
            {
                //alpha값 변경
                Color _color = _leftImage.color;
                _color.a = 0.3f;
                _leftImage.color = _color;
            }
            else
            {
                //alpha값 변경
                Color _color = _leftImage.color;
                _color.a = 1.0f;
                _leftImage.color = _color;
            }
        }
        else
        {
            _leftImage.gameObject.SetActive(false);
        }
    }

    private void Right_imageCheck(int index)
    {
        //왼쪽 이미지를 켜야한다면 그 아이디에 맞춰 켜기
        if (dialogueData[index].Right_id != 0)
        {
            _rightImage.sprite = _chitoImages[dialogueData[index].Right_id - 1];
            _rightImage.SetNativeSize();
            _rightImage.gameObject.SetActive(true);

            //화자인지 아닌지 체크
            if (dialogueData[index].Right_id != dialogueData[index].Speaker)
            {
                //alpha값 변경
                Color _color = _rightImage.color;
                _color.a = 0.3f;
                _rightImage.color = _color;
            }
            else
            {
                //alpha값 변경
                Color _color = _rightImage.color;
                _color.a = 1.0f;
                _rightImage.color = _color;
            }
        }
        else
        {
            _rightImage.gameObject.SetActive(false);
        }
    }

    private void selectbox()
    {
        dialogueData = CSVParsingD.GetDialogue("select");

        Debug.Log(dialogueData[0].Secletion_Context[0]);
        Debug.Log(dialogueData[0].Secletion_Context[1]);

        for (int i = 0; i < dialogueData[0].Secletion_Context.Length; i++)
        {
            //박스키고
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //글자 박고
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //다음 대사 알려주고
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    private void selectbox2()
    {
        dialogueData = CSVParsingD.GetDialogue("select2");

        for (int i = 0; i < dialogueData[0].Secletion_Context.Length; i++)
        {
            //박스키고
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //글자 박고
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //다음 대사 알려주고
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }
}
