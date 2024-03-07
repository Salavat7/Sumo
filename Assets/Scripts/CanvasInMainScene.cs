using TMPro;
using UnityEngine;

public class CanvasInMainScene : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _textMeshPro;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }


    private void Update()
    {
        _textMeshPro.text = _gameManager.PlayerName + " score: " + _gameManager.Score;
    }
}
