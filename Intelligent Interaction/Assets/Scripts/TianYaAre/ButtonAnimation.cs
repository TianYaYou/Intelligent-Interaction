using UnityEngine;

namespace TianYaAre.Ui
{
    public class ButtonAnimation : MonoBehaviour
    {
        public float animation_show_time = 0.5f; // 进入动画时间
        public float animation_hide_time = 0.5f; // 退出动画时间
        private float animation_late_show_time = 0f; // 延迟显示动画时间

        private float animation_used_time = 0f; // 已使用的动画时间
        public int animation_state = 0; // 动画状态：0 - 无，1 - 进入动画，2 - 退出动画

        public bool is_show = true; // 是否显示
        public bool late_show = true; // 是否延迟显示
        

        private Vector2 position; // 按钮位置

        static int active_button_count = 0; // 活动按钮计数器
        int id; // 按钮ID


        void Reset()
        {
            if (GetComponent<CanvasGroup>() is null)
            {
                // 如果没有 CanvasGroup 组件，则添加一个
                CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }


        CanvasGroup canvasGroup;

        void Start()
        {
            id = active_button_count; // 设置按钮ID为当前活动按钮计数器的值
            active_button_count++;
            if (late_show)
            {
                animation_late_show_time += (id)*0.25f;
            }

            if (GetComponent<CanvasGroup>() is null)
            {
                Debug.LogError("CanvasGroup component is missing on the GameObject. Please add a CanvasGroup component to enable animations.");
                return;
            }
            // 初始化 CanvasGroup 的透明度和交互性
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            //if (is_show)
            //{
            //    animation_state = 1; // 设置为进入动画状态
            //    position = transform.localPosition; // 获取初始位置
            //    transform.localPosition = new Vector2(position.x - 100, position.y); // 初始位置向左偏移
            //}

        }
        void Update()
        {
            if (animation_state != 0)
            {
                if (animation_state == 1)
                {
                    // 线性插值动画
                    animation_used_time += Time.deltaTime;
                    if (animation_used_time > 0)
                    {
                        if (animation_used_time >= animation_show_time)
                        {
                            animation_used_time = animation_show_time; // 确保不超过动画时间
                            animation_state = 0; // 动画完成，重置状态
                        }
                        float t_linear = animation_used_time / animation_show_time; // 线性插值系数
                                                                                    // 应用缓出效果：t 的平方根，使动画开始时快，结束时慢
                        float t_eased = Mathf.Sqrt(t_linear);

                        canvasGroup.alpha = Mathf.Lerp(0f, 1f, t_eased); // 透明度从0到1
                        GetComponent<RectTransform>().localPosition = Vector2.Lerp(new Vector2(position.x - 100, position.y), position, t_eased); // 从左侧移动到原位置
                    }
                }
                else if (animation_state == 2)
                {
                    // 线性插值动画
                    animation_used_time += Time.deltaTime;
                    if (animation_used_time > 0)
                    {
                        if (animation_used_time >= animation_hide_time)
                        {
                            animation_used_time = animation_hide_time; // 确保不超过动画时间
                            animation_state = 0; // 动画完成，重置状态
                            canvasGroup.alpha = 0f; // 设置透明度为0
                            gameObject.SetActive(false); // 隐藏游戏对象
                        }
                        float t_linear = animation_used_time / animation_hide_time; // 线性插值系数
                                                                                    // 应用缓入效果：t 的平方，使动画开始时慢，结束时快
                        float t_eased = t_linear * t_linear;

                        canvasGroup.alpha = Mathf.Lerp(1f, 0f, t_eased); // 透明度从1到0
                        GetComponent<RectTransform>().localPosition = Vector2.Lerp(position, new Vector2(position.x + 100, position.y), t_eased); // 从原位置移动到右侧
                    }
                }
            }
            else
            {
                if (is_show && GetComponent<CanvasGroup>().alpha == 0) Show();
            }
        }

        private void OnDestroy()
        {
            if(id!=-1) active_button_count--;
        }

        void Show()
        {
            animation_used_time = -animation_late_show_time;
            animation_state = 1; // 设置为进入动画状态
            position = GetComponent<RectTransform>().localPosition; // 获取初始位置
            GetComponent<RectTransform>().localPosition = new Vector2(position.x - 100, position.y); // 初始位置向左偏移
        }


        void Hide()
        {
            animation_used_time = -animation_late_show_time;
            animation_state = 2; // 设置为进入动画状态
            position = GetComponent<RectTransform>().localPosition; // 获取初始位置
        }
        public void DestroyIt(bool father = false)
        {
            GameObject father_obj = GetComponent<RectTransform>().parent.gameObject; // 获取父对象
            //移调画布跟坐标（不改变位置）
            GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), true); // 移除父对象，保持世界坐标不变
            Hide();
            if (father) Destroy(father_obj);
            //延迟销毁按钮对象
            Destroy(gameObject, animation_hide_time); // 在动画结束后销毁按钮对象
            id = -1;
            active_button_count--;
        }
    }
}
