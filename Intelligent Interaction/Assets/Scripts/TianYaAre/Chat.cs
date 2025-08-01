﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TianYaAre.Ui;
using TMPro;
using UnityEngine;

namespace TianYaAre.MainScene
{
    public class Chat : MonoBehaviour
    {
        public static IChatDataInterface Active_Instance;
        ChatDataList chatDataList;
        public GameObject button_preferb;
        public GameObject chatPanel;
        public GameObject chatButtonFather;

        public bool waiting_for_result = false;
        public bool set_gui = true;

        Task<ChatDataList> awaitChatDataList;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (Active_Instance is not null)
            {
                waiting_for_result = true;
                awaitChatDataList = Active_Instance.StartChat();
            }
        }

        BeforeChatData BeforeChatDataTemp = new BeforeChatData();

        private void Update()
        {
            if (set_gui)
            {
                if (waiting_for_result) 
                {
                    
                }
                else
                {
                    chatDataList = awaitChatDataList.Result;
                    chatPanel.GetComponentInChildren<TextMeshProUGUI>().text = chatDataList.return_chat;
                    BeforeChatDataTemp.return_chat = chatDataList.return_chat;
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
                            SaveData.beforeChatData.Add(BeforeChatDataTemp);
                            BeforeChatDataTemp.chat = chatDataList.list[index].data;
                            awaitChatDataList = Active_Instance.PalyerChange(index, BeforeChatDataTemp.chat);
                            waiting_for_result = true;
                            SetGui();
                        });
                    }
                    set_gui = false;
                }
            }
        }

        void SetGui()
        {
            set_gui = true;
        }
    }
}


public struct BeforeChatData
{
    public string return_chat;
    public string chat;
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

    /// <summary>
    /// 游戏开始时调用
    /// </summary>
    /// <returns></returns>
    public Task<ChatDataList> StartChat();

    /// <summary>
    /// 用户点击时调用
    /// </summary>
    /// <param name="ChangeIndix">点击的按钮索引</param>
    /// <returns></returns>
    public Task<ChatDataList> PalyerChange(int ChangeIndix, string change_text);
}