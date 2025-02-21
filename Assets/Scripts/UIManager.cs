using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _reload;

    private void Start()
    {
        _reload.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }
}
