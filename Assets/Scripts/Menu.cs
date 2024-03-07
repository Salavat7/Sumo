using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]private List<Material> _skins;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        _textMeshPro.text = _gameManager.BestPlayerName + " score: " + _gameManager.BestScore;
    }

    public void SelectFirstSkin()
    {
        _gameManager.CurrentSkin = _skins[0];
        SceneManager.LoadScene("Main");
    }

    public void SelectSecondSkin()
    {
        _gameManager.CurrentSkin = _skins[1];
        SceneManager.LoadScene("Main");
    }

    public void SelectThirdSkin()
    {
        _gameManager.CurrentSkin = _skins[2];
        SceneManager.LoadScene("Main");
    }

    public void SetPlayerName()
    {
        _gameManager.PlayerName = gameObject.GetComponentInChildren<TMP_InputField>().text;
    }
}
