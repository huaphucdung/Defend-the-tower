using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterManager", menuName = "Manager/Master Manager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
   [SerializeField] private AudioSetting _audioSetting;
   [SerializeField] private ScreenSetting _screenSetting;
   [SerializeField] private MouseSetting _mouseSetting;

   public static AudioSetting AudioSetting {get {return Instance("Manager")._audioSetting;}}
   public static ScreenSetting ScreenSetting {get {return Instance("Manager")._screenSetting;}}
   public static MouseSetting MouseSetting {get {return Instance("Manager")._mouseSetting;}}

   public static void SaveDataSetting() {
      SaveSettingData saveData = new SaveSettingData();

      saveData.fullScreenSave = ScreenSetting.fullScreen;
      saveData.resolutionSave = ScreenSetting.resolution;

      saveData.audioSave = AudioSetting.audio;
      saveData.sensitiveSave = MouseSetting.sensitive;

      string saveJson = JsonUtility.ToJson(saveData);
      PlayerPrefs.SetString("SaveData", saveJson);
      PlayerPrefs.Save(); 
   }

   public static void LoadDataSetting() {
      if(PlayerPrefs.HasKey("SaveData")) {
         string saveJson = PlayerPrefs.GetString("SaveData");
         SaveSettingData saveData = JsonUtility.FromJson<SaveSettingData>(saveJson);

         ScreenSetting.fullScreen = saveData.fullScreenSave;
         ScreenSetting.resolution = saveData.resolutionSave;

         AudioSetting.audio = saveData.audioSave;
         MouseSetting.sensitive = saveData.sensitiveSave;
      }
   }
}
