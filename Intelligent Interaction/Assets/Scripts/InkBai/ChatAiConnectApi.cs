using System;
using System.Net.Http;
using System.Text;
using System.IO;
using UnityEngine;

namespace InkBai.MainScene
{
    public class ChatAiConnectApi : MonoBehaviour
    {
        [Header("DeepSeek API Settings")]
        [Tooltip("你的 DeepSeek API Key。为了安全，请考虑从更安全的位置加载。")]
        public string apiKey = "sk-f8a47679216d47498042d31338ef1f3f"; // 替换为你的 DeepSeek API Key

        [Tooltip("DeepSeek API 的基础 URL。")]
        public string apiUrl = "https://api.deepseek.com/chat/completions";

        [TextArea(2, 5)]
        public string userPrompt = "给我讲个故事";

        private void Start()
        {

            SetPromit(Application.streamingAssetsPath + @"\SystemPrompts\1.txt");
            // 示例：启动时自动调用
            CallDeepSeekChat(userPrompt);
        }

        public string prompt;

        public bool ready = true;

        public string result;



        public void SetPromit(string prompt_txt_file = "")
        {
            //txt文件读取
            if (string.IsNullOrEmpty(prompt_txt_file))
            {
                Debug.LogWarning("请提供一个有效的提示文本文件路径。");
                return;
            }
            if (File.Exists(prompt_txt_file))
            {
                try
                {
                    prompt = File.ReadAllText(prompt_txt_file);
                    Debug.Log($"已加载提示文本: {prompt}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"读取提示文本文件失败: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"提示文本文件不存在: {prompt_txt_file}");
            }
        }

        public async void CallDeepSeekChat(string chat_prompt, string history = "")
        {
            if (!ready)
            {
                Debug.LogWarning("拥有一个正在进行的会话");
            }
            using (var client = new HttpClient())
            {
                try
                {
                    ready = false;
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Authorization", $"Bearer {apiKey}");

                    var json = $@"{{
                                 ""messages"": [
                                 {{
                                    ""content"": ""{EscapeJson(prompt)}"",
                                    ""role"": ""system""
                                  }},
                                  {{
                                    ""content"": ""{EscapeJson(history)}{EscapeJson(chat_prompt)}"",
                                    ""role"": ""user""
                                  }}
                                ],
                                ""model"": ""deepseek-chat"",
                                ""frequency_penalty"": 0,
                                ""max_tokens"": 5000,
                                ""presence_penalty"": 0,
                                ""response_format"": {{
                                  ""type"": ""text""
                                }},
                                ""stop"": null,
                                ""stream"": false,
                                ""stream_options"": null,
                                ""temperature"": 0.8,
                                ""top_p"": 1,
                                ""tools"": null,
                                ""tool_choice"": ""none"",
                                ""logprobs"": false,
                                ""top_logprobs"": null
                              }}";


/*
 ChatDataList事例json：
{
    "return_chat": "你好，欢迎来到天涯世界！请选择你的回复：",
    "list": [
        { "data": "你好！" },
        { "data": "请问这里是哪里？" },
        { "data": "我该做什么？" }
    ]
}
 */
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();

                    ready = true;
                    Debug.Log($"DeepSeek 响应: {result}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"DeepSeek API 调用失败: {ex.Message}");
                }
            }
        }

        // 简单的 JSON 字符串转义
        private string EscapeJson(string str)
        {
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }
    }

}
