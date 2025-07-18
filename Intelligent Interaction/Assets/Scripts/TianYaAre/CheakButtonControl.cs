using Unity.VisualScripting;
using UnityEngine;

namespace TianYaAre.ProgectChange
{

    public class CheakButtonControl : MonoBehaviour
    {
        public int target_txt = 1; // 目标文本

        public void ButtonKick()
        {
            SaveData.role_indix = target_txt; // 设置角色索引为目标文本
            //更换场景到Main
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
        private void Start()
        {
            //链接按钮点击事件
            if (GetComponent<UnityEngine.UI.Button>() is not null)
            {
                GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonKick);
            }
            else
            {
                Debug.LogError("Button component is missing on the GameObject. Please add a Button component to enable button functionality.");
            }
        }
    }
    
}