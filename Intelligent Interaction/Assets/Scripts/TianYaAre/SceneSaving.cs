using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace TianYaAre.MainScene
{
    public class SceneSaving : MonoBehaviour
    {
        public static string load_path = "";

        /// <summary>
        /// 是否是加载存档
        /// </summary>
        public static bool change_from_loder = false;

        private void Start()
        {
            if (change_from_loder)
            {
                LoadAll();
            }
        }

        public void SaveAll()
        {
            //在Assets/StreamingAssets/SceneSaving/<日期 y-m-d-h-m-t>.json新建一个文件
            try
            {
                //反序列SaveData的数据到一个JSON文件
                _saveData saveData = new _saveData();
                saveData.GetSaveData();
                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                string time = $"{System.DateTime.Now:yyyy-MM-dd-HH-mm-ss}";
                string fileName = $"SceneSaving/{time}.json";
                string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // 确保目录存在
                File.WriteAllText(fullPath, json);
                if (load_path != "")
                {
                    //删除之前的文件
                    if (File.Exists(load_path))
                    {

                        string old_fullPath = Path.Combine(Application.streamingAssetsPath, $@"SceneSaving/{load_path}.json");
                        File.Delete(old_fullPath);
                        Debug.Log($"已删除旧的保存文件: {load_path}");
                    }
                }
                load_path = time;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"保存数据失败: {e.Message}");
            }
        }
        public void LoadAll()
        {
            try
            {
                if (load_path == "")
                {
                    Debug.LogWarning("没有指定加载路径，请先保存数据。");
                    return;
                }
                string fileName = $"SceneSaving/{load_path}.json";
                string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
                if (File.Exists(fullPath))
                {
                    string json = File.ReadAllText(fullPath);
                    _saveData saveData = JsonConvert.DeserializeObject<_saveData>(json);
                    saveData.SetSaveData();
                    Debug.Log($"已成功加载保存数据: {load_path}");
                }
                else
                {
                    Debug.LogWarning($"保存文件不存在: {fullPath}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载数据失败: {e.Message}");
            }
        }
        public void ReturnToMain()
        {
            change_from_loder = false; // 重置加载状态
            load_path = ""; // 清空加载路径
            SaveData.beforeChatData.Clear(); // 清空保存数据
            SaveData.role_indix = 1; // 重置角色索引
            //返回主场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }

    public class _saveData
    {
        public List<BeforeChatData> beforeChatData = new List<BeforeChatData>();
        public int role_indix = 1;
        public void SetSaveData()
        {
            SaveData.beforeChatData = beforeChatData;
            SaveData.role_indix = role_indix;
        }
        public void GetSaveData()
        {
            beforeChatData = SaveData.beforeChatData;
            role_indix = SaveData.role_indix;
        }
    }
}

public static class SaveData
{
    static public List<BeforeChatData> beforeChatData = new List<BeforeChatData>();
    static public int role_indix = 1;
}
