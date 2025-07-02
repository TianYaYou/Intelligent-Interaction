using UnityEngine;

namespace TianYaAre.StartScene
{
    public class StartScene : MonoBehaviour
    {
        void ChangeSceneToMian()
        {
            // 切换到主场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}
