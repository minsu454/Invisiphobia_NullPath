using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Common.Event;



#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

public class CameraController : MonoBehaviour
{
    public bool isSprinting = false;

    #region Camera Movement
    public Camera playerCamera;
    //new를 통한 garbage를 방지하기 위한 변수
    private Vector3 playerRotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    //플레이어가 맵을 볼때 얼마나 넓게 볼지를 결정. = fieldOfView
    //작아지면 좁고 가깝게 넓어지면 멀리 볼 수 있게
    public float fov = 60f;
    public bool cameraCanMove = true;
    //마우스의 감도(높을 수록 빨라짐)
    public float mouseSensitivity = 2f;
    //위아래의 최대 볼 수 있는 각도
    public float maxLookAngle = 50f;

    // Crosshair(Aim)
    //마우스 잠금 여부
    public bool lockCursor = true;
    //에임 활성화 할지 여부
    public bool crosshair = true;
    //실제로 에임을 구현할 이미지 오브젝트
    private Image crosshairObject;
    //에임에 들어갈 이미지(플레이어에게 보이는 외관)
    public Sprite crosshairImage;

    // Internal Variables
    //좌우 회전
    private float yaw = 0.0f;
    //위아래 회전
    private float pitch = 0.0f;

    #endregion


    #region Camera Zoom Variables
    //카메라를 줌할지 여부
    public bool enableZoom = true;
    //어떠한 버튼을 조건으로 지정하고 꾹 눌렀을때 줌할지 여부
    public bool holdToZoom = false;
    //zoomKey라는 변수에 Mouse1(마우스 오른쪽버튼) 할당
    public KeyCode zoomKey = KeyCode.Mouse1;
    //줌을 했을 때 얼마나 넓게 볼지를 결정
    //값이 작아지면 더 가까이보고 값이 커지면 더 멀리서봄.
    public float zoomfov = 30f;
    //줌을 할때 걸리는 시간(줌을 다 하기까지 걸리게 할 시간)
    public float zoomStepTime = 5f;

    // Internal Variables
    //줌을 했는지 확인하는 조건문
    public bool isZoomed = false;

    #endregion

    public void Init(Player player)
    {
        //에임 오브젝트에 이미지 컴포넌트를 자식으로 붙여라.
        crosshairObject = GetComponentInChildren<Image>();
        // Set internal variables
        //fov 변수에 플레이어 카메라의 fieldOfView 기능을 추가해라
        playerCamera.fieldOfView = fov;

        EventManager.Subscribe(GameEventType.UsePause, OnSetLockOff);
    }

    void Start()
    {
        
        //커서 잠금 기능(인게임에서 마우스 안나오게)
        if (lockCursor)
        {
            SetLock();
        }
        //에임기능
        if (crosshair)
        {
            //이미지 오브젝트에 해당 sprite이미지를 넣음.
            //crosshairObject.sprite = crosshairImage;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 마우스 잠구는 코드
    /// </summary>
    public void SetLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraCanMove = true;
    }

    /// <summary>
    /// 마우스 살리는 코드
    /// </summary>
    public void SetLockOff()
    {
        Cursor.lockState = CursorLockMode.None;
        cameraCanMove = false;
    }

    private void OnSetLockOff(object args)
    {
        SetLockOff();
    }

    //캠 회전
    float camRotation;

    private void Update()
    {
        CameraRotation();
        CameraZoom();
    }

    private void CameraRotation()
    {
        #region Camera

        // Control camera movement
        //카메라를 움직일 수 있으면
        if (cameraCanMove)
        {
            //좌우회전 = 로컬좌표y축을 기준으로 MouseX 좌표에 감도를 곱해서 움직임
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            //위아래 회전 -= 감도 * MouseY좌표 이동으로 움직임.
            pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

            //pitch(위아래)회전 값을 제한
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            //transform.localEulerAngles = new Vector3(0, yaw, 0);
            //garbage를 생성하는 new 대신에 클래스 내에 캐싱된 vector3변수를 재사용.
            playerRotation.y = yaw;
            transform.localEulerAngles = playerRotation;

            //playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            //위와 동일.
            cameraRotation.x = pitch;
            playerCamera.transform.localEulerAngles = cameraRotation;

            //왜 좌우 회전은 플레이어 기준으로 회전하고
            //상하 회전은 마우스를 기준으로 회전하는가?

            //좌우 회전을 플레이어 기준으로 하면(카메라가 회전할 때 플레이어도 같이 회전하면)
            //플레이어의 wasd움직임이 카메라에 동기화되어서 w를 누를 때 카메라 기준으로 앞으로
            //가지게 된다.

            //상하 회전을 카메라 기준으로 한 이유는 상하회전을 플레이어 기준으로 하게되면
            //마우스가 위를 보거나 아래를 볼때 플레이어 오브젝트 자체가 누워버리는 현상이
            //발생하게 되기 때문에 카메라를 기준으로 카메라만 회전시켜서(카메라가 고개 역할)
            //자연스러운 움직임을 구현한 것이다.
            #endregion
        }
    }
    private void CameraZoom()
    {
        #region Camera Zoom
        //Zoom기능이 활성화 되었을 경우(bool, base = true)
        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            //마우스 오른쪽 버튼을 누르고 holdToZoom이 true이며 달리는중이 아닐 경우
            //holdToZoom false = 한번 클릭만으로 줌동작.
            if (Input.GetKeyDown(zoomKey) && !holdToZoom)
                //zoomKey를 꾹 눌러서 zoom
            {
                if (!isZoomed)
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (holdToZoom)
            //zoomKey를 한번만 눌러서 zoom
            {
                if (Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if (Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
            {
                //playercamera의 fov를 zoomed가 될 때 zoomStepTime에 따라 부드럽게 확장되게 하는 코드
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomfov, zoomStepTime * Time.deltaTime);
            }
            //zoom이 꺼지고 달리는 중이 아닐 경우
            else if (!isZoomed && !isSprinting)
            {
                //playercamera의 fov를 zoomed가 될 때 zoomStepTime에 따라 부드럽게 축소되게 하는 코드
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
            #endregion

        }
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.UsePause, OnSetLockOff);
    }
}

        


