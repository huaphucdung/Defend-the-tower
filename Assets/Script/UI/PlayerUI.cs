using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject targetCrosshair;
    [SerializeField] private Slider HealthBarSlider;
    [SerializeField] private TMP_Text HealthBarText;

    [SerializeField] private Image Skill;
    
    [SerializeField] private Slider TowerSlider;
    [SerializeField] TowerDefense tower;

    [SerializeField] private GameObject rescueImage;
    //Variable for target
    private Transform _target;
    private Vector2 defaulSize;
    private Image _imageTarget;
    private PlayerController PC;
    private CharacterClass CharacterInfo;
    // Start is called before the first frame update
    void Start()
    {
        _imageTarget = targetCrosshair.GetComponent<Image>(); //.rectTransform.sizeDelta = new Vector2(100, 200);
        targetCrosshair.SetActive(false);
        defaulSize = _imageTarget.rectTransform.sizeDelta;

        //Get PV of character which player can control
        PhotonView[] list_PV = FindObjectsOfType<PhotonView>();
        foreach(PhotonView view in list_PV) {
            if(view.IsMine) {
                PC = view.gameObject.GetComponent<PlayerController>();
                break;
            }
        }
        
        HealthBarSlider.maxValue = PC.MaxHealth;
        CharacterInfo = PC.GetCharacterClass();
        TowerSlider.maxValue = tower.MaxHealthTower;
    }

    void Update() {
        //Udpate healthbar value
        HealthBarSlider.value = PC.Health;
        HealthBarText.text = PC.Health.ToString()+"/"+PC.MaxHealth.ToString();

        //Update skill value
        float coutdown = PC.ResetSkill - Time.time;
        if(coutdown > CharacterInfo.TimeResetSkill) Skill.fillAmount = 0;
        else if(coutdown <=  0) Skill.fillAmount = 1;
        else {
           Skill.fillAmount = 1 - coutdown/CharacterInfo.TimeResetSkill;
        }

        //Update healthbar tower
        TowerSlider.value = tower.HealthTower;
    }

    public bool ShowTargetCrosshair(Camera mainCamera,Transform playerTran, Transform _target, float radiusScan) {      
        //Check target in view of camera and in player's target distance 
        if(_target != null && Vector3.Distance(playerTran.position, _target.position) <= radiusScan) {
            Vector3 _targetPos = mainCamera.WorldToViewportPoint(_target.position);
            //Check _targin in screen view show crosshair follow target
            if(_targetPos.z > 0 && _targetPos.x >0 && _targetPos.x < 1 && _targetPos.y > 0 && _targetPos.y < 1) {
                targetCrosshair.SetActive(true);
                _imageTarget.rectTransform.sizeDelta = defaulSize - new Vector2(5,5) * _targetPos.z;
                targetCrosshair.transform.position = mainCamera.WorldToScreenPoint(_target.position + _target.up * 0.5f);                    
                return true;
            }      
        }
        targetCrosshair.SetActive(false);
        return false;
    }

    public void ShowRescue(bool value) {
        rescueImage.SetActive(value);
    }
}
