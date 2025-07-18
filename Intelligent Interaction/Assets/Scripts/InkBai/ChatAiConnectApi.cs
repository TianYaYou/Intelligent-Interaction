using System;
using System.Net.Http;
using System.Text;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

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

        public string prompt;

        public bool ready = true;

        public string result;

        public ReturnChatData resultData = new ReturnChatData();

        public GameObject UiMask;

        public string result_text_only
        {
            get
            {
                if (resultData.choices != null && resultData.choices.Length > 0)
                {
                    return resultData.choices[0].message.content;
                }
                Debug.LogWarning("结果数据中没有可用的文本内容。请检查 API 响应。");
                return string.Empty;
            }
        }

        void SetResultData()
        {
            //result反序列化
            if (string.IsNullOrEmpty(result))
            {
                Debug.LogWarning("结果字符串为空，无法反序列化。");
                return;
            }
            try
            {
                resultData = JsonConvert.DeserializeObject<ReturnChatData>(result);
                Debug.Log($"已成功反序列化结果数据: {resultData.id}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"反序列化结果数据失败: {ex.Message}");
            }
        }

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

                    UiMask?.SetActive(true);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();

                    SetResultData();
                    UiMask?.SetActive(false);
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
    /*
    {
      "id": "4e44564a-ab84-4731-b8ff-ceb6ce1243bc",
      "object": "chat.completion",
      "created": 1752762230,
      "model": "deepseek-chat",
      "choices": [
        {
          "index": 0,
          "message": {
            "role": "assistant",
            "content": "{\n    \"return_chat\":\"哎呦喂！来新客人啦！张大爷我正抱着酒坛子打盹呢，被你这声招呼惊得差点把酒洒喽！来来来，让老汉我瞅瞅——你是想学酿酒啊，还是想尝尝我这‘醉倒神仙’的雄黄酒？\",\n    \"list\": [\n        { \"data\": \"想学酿酒\" },\n        { \"data\": \"尝尝雄黄酒\" },\n        { \"data\": \"您这酒坊历史\" }\n    ]\n}"
          },
          "logprobs": null,
          "finish_reason": "stop"
        }
      ],
      "usage": {
        "prompt_tokens": 416,
        "completion_tokens": 115,
        "total_tokens": 531,
        "prompt_tokens_details": { "cached_tokens": 384 },
        "prompt_cache_hit_tokens": 384,
        "prompt_cache_miss_tokens": 32
      },
      "system_fingerprint": "fp_8802369eaa_prod0623_fp8_kvcache"
    }
      */
    public class ReturnChatData
    {
        public string id;
        public string @object;
        public long created;
        public string model;
        public Choice[] choices;
        public Usage usage;
        public string system_fingerprint;

        public class Choice
        {
            public int index;
            public Message message;
            public object logprobs;
            public string finish_reason;

            public class Message
            {
                public string role;
                public string content;
            }
        }
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
            public PromptTokensDetails prompt_tokens_details;
            public int prompt_cache_hit_tokens;
            public int prompt_cache_miss_tokens;

            public class PromptTokensDetails
            {
                public int cached_tokens;
            }
        }
    }
}
