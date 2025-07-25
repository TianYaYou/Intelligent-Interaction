﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TianYaAre.MainScene;
using UnityEngine;

namespace InkBai.MainScene
{
    public class ChatMain : MonoBehaviour, IChatDataInterface
    {
        public Chat chat;
        public ChatAiConnectApi connectApi;

        public async Task<ChatDataList> StartChat()
        {
            connectApi.SetPromit(Application.streamingAssetsPath + @$"\SystemPrompts\{SaveData.role_indix}.txt");
            connectApi.CallDeepSeekChat("");
            ChatDataList chatDataList = new ChatDataList();

            while (!connectApi.ready)
            {
                await Task.Delay(100); // 等待API响应
            }
            chat.waiting_for_result = false;
            string result_text_only = connectApi.result_text_only;
            try
            {
                //反序化结果为ChatDataList对象
                chatDataList = JsonConvert.DeserializeObject<ChatDataList>(result_text_only);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"反序列化ChatDataList失败: {ex.Message}");
                chatDataList.return_chat = "对不起，发生了错误，请稍后再试。";
                chatDataList.list = new List<ChatData>();
                return chatDataList;
            }
            return chatDataList;
        }
        public async Task<ChatDataList> PalyerChange(int ChangeIndix, string change_text)
        {
            string history = JsonConvert.SerializeObject(SaveData.beforeChatData);
            connectApi.CallDeepSeekChat(change_text, history);
            ChatDataList chatDataList = new ChatDataList();
            while (!connectApi.ready)
            {
                await Task.Delay(100); // 等待API响应
            }
            chat.waiting_for_result = false;
            string result_text_only = connectApi.result_text_only;
            try
            {
                //反序化结果为ChatDataList对象
                chatDataList = JsonConvert.DeserializeObject<ChatDataList>(result_text_only);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"反序列化ChatDataList失败: {ex.Message}");
                chatDataList.return_chat = "对不起，发生了错误，请稍后再试。";
                chatDataList.list = new List<ChatData>();
                return chatDataList;
            }
            return chatDataList;
        }
        private void Start()
        {

            TianYaAre.MainScene.Chat.Active_Instance = this;
        }
    }
}