using System;
using System.Net.Http;
using System.Text;
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
        public string userPrompt = "Hi";

        private void Start()
        {
            // 示例：启动时自动调用
            CallDeepSeekChat(userPrompt);
        }

        public string result;

        public async void CallDeepSeekChat(string prompt)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Authorization", $"Bearer {apiKey}");

                    var json = $@"{{
                                 ""messages"": [
                                {{
                                    ""content"": ""你是一只可爱的小猫娘"",
                                    ""role"": ""system""
                                  }},
                                  {{
                                    ""content"": ""{EscapeJson(prompt)}"",
                                    ""role"": ""user""
                                  }}
                                ],
                                ""model"": ""deepseek-chat"",
                                ""frequency_penalty"": 0,
                                ""max_tokens"": 1000,
                                ""presence_penalty"": 0,
                                ""response_format"": {{
                                  ""type"": ""text""
                                }},
                                ""stop"": null,
                                ""stream"": false,
                                ""stream_options"": null,
                                ""temperature"": 1,
                                ""top_p"": 1,
                                ""tools"": null,
                                ""tool_choice"": ""none"",
                                ""logprobs"": false,
                                ""top_logprobs"": null
                              }}";

                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
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
