using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private SorceManage sorceManage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private HpManage hpManage;
    [SerializeField] private List<GameObject> ImageInfo=new List<GameObject>();
    private void Start()
    {
        if(board!=null)
        {
            board.onGameOver += HandleGameOver;
        }
        if(restartButton!=null)
        {
            restartButton.onClick.AddListener(Restartgame);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        if(gameOverPanel!=null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
    private void HandleGameOver()
    {
        gameOverPanel.SetActive(true);
        if (hpManage != null)
        {
            hpManage.resetHpImage();
        }
        foreach (var i in ImageInfo)
        {
            i.SetActive(false);
        }
    }
    private void Restartgame()
    {
        foreach (var i in ImageInfo)
        {
            i.SetActive(true);
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (sorceManage != null)
        {
            sorceManage.ResetScore();
        }
        if(board!=null)
        {
            board.resetboard();
        }
        Hp.resetHp();
        Time.timeScale = 1f;
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在构建的游戏版本中，退出应用程序
        Application.Quit();
#endif
    }
    private void OnDestroy()
    {
        if(board!=null)
            board.onGameOver -= HandleGameOver;
        if(restartButton!=null)
            restartButton.onClick.RemoveListener(Restartgame);
        if (quitButton != null)
            quitButton.onClick.RemoveListener(QuitGame);

    }
}
