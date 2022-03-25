using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class VRCameraController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float turnSpeedMouse = 1f;
    private bool gyroEnable = false;
    
    float horizontal;
    float vertical;
    public Transform container;
    public Transform camera;

    private Quaternion containerQuat;
    private Quaternion cameraQuat;
    private Quaternion rot;

    [SerializeField]
    private List<Texture2D> textures = new List<Texture2D>();

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Slider slider;

    private static VRCameraController instance;
    public static VRCameraController Instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<VRCameraController>();
        }

        return instance;
    }
    private void Start()
    {

        instance = this;
        UpdateSlider(100);
        Resources.UnloadUnusedAssets();
        
    }

    private void Update()
    {
        if(!gyroEnable) return;
        Quaternion q = Input.gyro.attitude;
        Quaternion qq = Quaternion.AngleAxis(90f, Vector3.right);
        camera.transform.localRotation =qq* q*rot;

    }


    public void GyroON(bool value)
    {
        Input.gyro.enabled = value;
        if (value)
        {
            rot = new Quaternion(0, 0, 1, 0);
        }
        gyroEnable = value;
        
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        containerQuat = container.localRotation;
        cameraQuat = camera.localRotation;
    }

    // ドラック中に呼ばれる.
    public void OnDrag(PointerEventData eventData)
    {
        
        container.Rotate(new Vector3(0, eventData.delta.x, 0) * Time.deltaTime * turnSpeedMouse);
        camera.Rotate(new Vector3(eventData.delta.y, 0, 0) * Time.deltaTime * turnSpeedMouse);
        
        camera.localEulerAngles = new Vector3(camera.localEulerAngles.x, 0, 0);
    }

    // ドラックが終了したとき呼ばれる.
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void ChangeTex(int val)
    {
        if (textures.Count > val)
        {
            var renderer = container.gameObject.GetComponent<Renderer>();
            var material = renderer.material;
            Debug.Log(material.GetTexture("_BaseMap"));
        
            material.SetTexture("_BaseMap",textures[val]);

            Debug.Log(textures[val].name);
            Resources.UnloadUnusedAssets();
        }
        
    }

    public void UpdateSlider(float val)
    {
        camera.gameObject.GetComponent<Camera>().fieldOfView = val;
    }
}
