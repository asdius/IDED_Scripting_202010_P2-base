using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image[] lifeImages = new Image[3];

    [SerializeField] private Text scoreLabel = null;

    [SerializeField] private Button restartBtn = null;

    private void Start()
    {
        ToggleRestartButton(false);

        Player.OnPlayerHit += UpdateLifes;
        Player.OnPlayerScoreChanged += UpdateScore;

        Player.OnPlayerDied += HandleOnPlayerDie;
    }

    private void HandleOnPlayerDie()
    {
        UpdateScore(0);
        ToggleRestartButton(true);
    }

    private void ToggleRestartButton(bool _value)
    {
        if (restartBtn != null)
        {
            restartBtn.gameObject.SetActive(_value);
        }
    }

    private void UpdateLifes()
    {
        for (int i = lifeImages.Length - 1; i >= 0; i--)
        {
            if (lifeImages[i].enabled)
            {
                lifeImages[i].enabled = false;
                return;
            }
        }
    }

    private void UpdateScore(int _score)
    {
        Debug.Log($"Campi, mi score es: {_score}");
        scoreLabel.text = _score.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        Player.OnPlayerHit -= UpdateLifes;
        Player.OnPlayerScoreChanged -= UpdateScore;

        Player.OnPlayerDied -= HandleOnPlayerDie;
    }
}