using System.Collections.Generic;
using UnityEngine;

namespace InkBai.MainScene
{
    public class ChatMain : MonoBehaviour, IChatDataInterface
    {
        public ChatDataList StartChat()
        {
            ChatDataList chatDataList = new ChatDataList();
            chatDataList.return_chat = "欢迎来到喵~！\n请选择你想要进行的对话：";
            chatDataList.list = new List<ChatData>
            {
                new ChatData { data = "你好" },
                new ChatData { data = "再见" },
                new ChatData { data = "你是谁？" }
            };
            return chatDataList;
        }
        public ChatDataList PalyerChange(int ChangeIndix)
        {
            ChatDataList chatDataList = new ChatDataList();
            chatDataList.return_chat = $"你选择了第{ChangeIndix}选项喵~！\n请选择你想要进行的对话：";
            chatDataList.list = new List<ChatData>
            {
                new ChatData { data = "喵，好可爱" },
                new ChatData { data = "天涯最可爱了" },
                new ChatData { data = "喵？" }
            };
            return chatDataList;
        }
        private void Start()
        {
            TianYaAre.MainScene.Chat.Active_Instance = this;
        }
    }
}