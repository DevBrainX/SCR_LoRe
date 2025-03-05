using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance { get { return _instance; } }

    [SerializeField] GameManager _Game;
    [SerializeField] UiManager _Ui;
    [SerializeField] SceneManagerEx _Scene;
    [SerializeField] ResourceManagerEx _Resource;

    public static GameManager Game { get { return Instance._Game; } }
    public static UiManager Ui { get { return Instance._Ui; } }
    public static SceneManagerEx Scene { get { return Instance._Scene; } }
    public static ResourceManagerEx Resource { get { return Instance._Resource; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        Logger.Log("Managers Awake()");
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    void OnApplicationQuit()
    {
        //if (TcpClient != null)
        //{
        //    TcpClient.SendScore();
        //}

        //Debug.Log("score:" + Game.GetScore() + " elapsedTime:" + Game.GetElapsedTime());
    }
}
