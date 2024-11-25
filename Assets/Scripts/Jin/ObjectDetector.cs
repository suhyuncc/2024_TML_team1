using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    // Observer 생성
    public static ObjectDetector Instance;

    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;
    [SerializeField]
    private TowerMerge towerMerge;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    [SerializeField]
    private bool isMerge = false;

    private void Awake()
    {
        // "MainCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
        mainCamera = Camera.main;
        Instance = this;
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            Debug.Log("return");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
            // ray.origin : 광선의 시작위치(=카메라 위치)
            // ray.direction : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                if (hit.transform.CompareTag("Tile"))
                {
                    // 타워를 생성하는 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    // 머지모드일 때 타워 클릭
                    if (isMerge)
                    {
                        towerMerge.AddTower(hit.transform);
                        // 이미 선택된 타워임을 체크
                        hit.transform.GetComponent<TowerWeapon>().Is_Selected = true;
                    }
                    // 머지모드가 아닐 때 타워 클릭
                    else
                    {
                        towerDataViewer.OnPanel(hit.transform);
                    }

                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                towerDataViewer.OffPanel();

                if (isMerge)
                {
                    // 머지모드 해제
                    isMerge = false;
                    towerMerge.MergeCancel();
                }
            }

            hitTransform = null;
        }
    }

    public void SetMergeMode()
    {
        isMerge = true;
    }

    public void ResetMergeMode()
    {
        isMerge = false;
    }
}
