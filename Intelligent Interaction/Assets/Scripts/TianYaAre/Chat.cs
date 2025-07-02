using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TianYaAre.Ui;

namespace TianYaAre.MainScene
{
    public class Chat : MonoBehaviour
    {
        public static IChatDataInterface Active_Instance;
        ChatDataList chatDataList;
        public GameObject button_preferb;
        public GameObject chatPanel;
        public GameObject chatButtonFather;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (Active_Instance is not null)
            {
                chatDataList = Active_Instance.StartChat();
                SetGui();
            }
        }

        void SetGui()
        {
            chatPanel.GetComponentInChildren<TextMeshProUGUI>().text = chatDataList.return_chat;
            foreach (Transform child in chatButtonFather.transform)
            {
                if (child.GetComponentInChildren<ButtonAnimation>() is not null)
                {
                    child.GetComponentInChildren<ButtonAnimation>().DestroyIt(true);
                }
                else Destroy(child.gameObject);
            }
            for (int i = 0; i < chatDataList.list.Count; i++)
            {
                GameObject button = Instantiate(button_preferb, chatButtonFather.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = chatDataList.list[i].data;
                int index = i;
                button.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    chatDataList = Active_Instance.PalyerChange(index);
                    SetGui();
                });
            }
        }
    }
}

public class ChatDataList
{
    public string return_chat;
    public List<ChatData> list;
}
public class ChatData
{
    public string data;
}

public interface IChatDataInterface
{
    public SaveData saveData { get; set; }

    /// <summary>
    /// 游戏开始时调用
    /// </summary>
    /// <returns></returns>
    public ChatDataList StartChat();

    /// <summary>
    /// 用户点击时调用
    /// </summary>
    /// <param name="ChangeIndix">点击的按钮索引</param>
    /// <returns></returns>
    public ChatDataList PalyerChange(int ChangeIndix);
}