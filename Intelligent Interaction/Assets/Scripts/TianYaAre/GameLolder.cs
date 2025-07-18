using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TianYaAre.MainScene
{
    public class GameLolder : MonoBehaviour
    {

        public GameObject load_button_preferb;  // 按钮预制体

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //检查在Assets/StreamingAssets/SceneSaving下的所有json文件
            string[] files = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/SceneSaving", "*.json");
            foreach (string file in files)
            {
                //获取文件名
                string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                //创建一个按钮
                GameObject load_button = Instantiate(load_button_preferb, transform);
                load_button.name = fileName; // 设置按钮名称为文件名
                load_button.GetComponentInChildren<TextMeshProUGUI>().text = fileName; // 设置按钮文本为文件名
                load_button.GetComponentInChildren<Button>().onClick.AddListener(() => LoadSaves(fileName)); // 添加点击事件
            }
        }
        void LoadSaves(string load_path)
        {
            SceneSaving.load_path = load_path; // 设置加载路径
            SceneSaving.change_from_loder = true; // 设置为加载存档
            //切换场景到Main
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}