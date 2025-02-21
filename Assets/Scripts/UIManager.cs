using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _next;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _reload;
    [SerializeField] private Camera _camera;

    private int _level;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Level"))
            _level = PlayerPrefs.GetInt("Level");
        else
            PlayerPrefs.SetInt("Level", 0);
        _levelText.text = $"Level {_level + 1}";
        var carPlacer = GetComponent<CarPlacer>();
        carPlacer.Width += _level;
        carPlacer.Height += _level;
        carPlacer.CarCount += _level * 3;
        _camera.transform.position += new Vector3(_level * 5, _level * 23, -_level * 2);
    }

    private void Start()
    {
        _next.onClick.AddListener(() => NextLevel());
        _reload.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }

    public void NextLevel()
    {
        _level++;
        PlayerPrefs.SetInt("Level", _level);
        SceneManager.LoadScene("Game");
    }
}
