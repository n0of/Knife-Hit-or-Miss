using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject kunaiObject;
    public GameObject logObject;
    public GameObject levelObject;
    public GameState gameState;

    private GameObject _currentLevel;

    void Start()
    {
        var topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        var height = bottomRight.y - topLeft.y;
        var width = bottomRight.x - topLeft.x;
        var logHeight = logObject.GetComponent<SpriteRenderer>().bounds.size.y;
        var kunaiHeight = kunaiObject.GetComponent<SpriteRenderer>().bounds.size.y;
        var scale = Mathf.Min(width / (logHeight + 2 * kunaiHeight), 1);
        logHeight *= scale;
        kunaiHeight *= scale;
        scale -= 1;
        gameState = new GameState(scale, kunaiHeight, logHeight, height, new Vector3(0, -height / 2 + kunaiHeight / 2 + StaticVariables.KunaiMargin, 0));
        StartLevel(true);
    }

    void Update()
    {
    }

    public void StartLevel(bool isWin)
    {
        if (isWin)
        {
            gameState.Stage++;
            Debug.Log("Next level!");
        }
        else
        {
            Debug.Log($"Fin! Current level is {gameState.Stage}, apples hit {gameState.Apples}");
            gameState.Stage = 0;
        }
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        _currentLevel = Instantiate(levelObject, new Vector3(), Quaternion.identity);
        _currentLevel.name = StaticVariables.LevelObjectName;
    }
}

public class GameState
{
    public GameState(float scale, float kunaiHeight, float logHeight, float screenHeight, Vector3 startKunaiPosition)
    {
        Scale = scale;
        KunaiHeight = kunaiHeight;
        LogHeight = logHeight;
        ScreenHeight = screenHeight;
        StartKunaiPosition = startKunaiPosition;
    }
    public float Scale { get; }
    public float KunaiHeight { get; }
    public float LogHeight { get; }
    public float ScreenHeight { get; }
    public Vector3 StartKunaiPosition { get; }
    public int Stage { get; set; } = -1;
    public int Apples { get; set; }
}
