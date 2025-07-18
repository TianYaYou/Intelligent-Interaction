using UnityEngine;

namespace TianYaAre.StartScene
{
    public class StartScene : MonoBehaviour
    {
        public void ChangeSceneToCheak()
        {
            // 切换到主场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("ProjectChange");
        }
    }
}
