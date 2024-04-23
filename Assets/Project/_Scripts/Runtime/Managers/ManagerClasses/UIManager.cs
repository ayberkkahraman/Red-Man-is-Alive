using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text CoinCount;

        public void UpdateCoinText(int coinCount)
        {
            CoinCount.text = coinCount.ToString();
        }
        
        public void LoadCoinText()
        {
            CoinCount.text = SaveManager.LoadData("Coin", 0).ToString();
        }
    }
}
