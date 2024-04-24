using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class SaveManager : MonoBehaviour
  {
    #region Save / Load

    /// <summary>
    /// Save data with given key value
    /// </summary>
    public static void SaveData<T>(string key, T saveData)
    {
      // string gameDataJson = DataBehaviour.Serialize(saveData);
      // PlayerPrefs.SetString(key, gameDataJson);
    }

    /// <summary>
    /// Load data with save key saved earlier
    /// </summary>
    // public static T LoadData<T>(string key, T defaultData)
    // {
    //   if (!PlayerPrefs.HasKey(key))
    //   {
    //     Debug.Log($">>{key}<< has not found in datas...");
    //     SaveData(key, defaultData);
    //     return defaultData;
    //   }
    //   
    //   string gameDataJson = PlayerPrefs.GetString(key);
    //   return DataBehaviour.DeSerialize<T>(gameDataJson);
    // }

    #endregion
  }
}
