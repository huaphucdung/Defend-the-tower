using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject targetCrosshair;

    //Variable for target
    private Transform _target;
    private Vector2 defaulSize;
    private Image _imageTarget;
    // Start is called before the first frame update
    void Start()
    {
        _imageTarget = targetCrosshair.GetComponent<Image>(); //.rectTransform.sizeDelta = new Vector2(100, 200);
        targetCrosshair.SetActive(false);
        defaulSize = _imageTarget.rectTransform.sizeDelta;
    }


    public bool ShowTargetCrosshair(Camera mainCamera,Transform playerTran, Transform _target, float radiusScan) {      
        //Check target in view of camera and in player's target distance 
        if(_target != null && Vector3.Distance(playerTran.position, _target.position) <= radiusScan) {
            Vector3 _targetPos = mainCamera.WorldToViewportPoint(_target.position);
            //Check _targin in screen view show crosshair follow target
            if(_targetPos.z > 0 && _targetPos.x >0 && _targetPos.x < 1 && _targetPos.y > 0 && _targetPos.y < 1) {
                targetCrosshair.SetActive(true);
                _imageTarget.rectTransform.sizeDelta = defaulSize - new Vector2(5,5) * _targetPos.z;
                targetCrosshair.transform.position = mainCamera.WorldToScreenPoint(_target.position);                    
                return true;
            }      
        }
        targetCrosshair.SetActive(false);
        return false;
    }
}
