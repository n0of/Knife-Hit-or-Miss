using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public GameObject kunaiObject;
    public GameObject logObject;
    public GameObject appleObject;
    public AppleScriptableObject appleScriptableObject;

    private GameScript _gameScript;
    private GameObject _spawnedApple;
    private int _stuckKunai = 0;
    private int _kunaiId = 0;

    void Start()
    {
        var gameScriptObject = GameObject.Find("GameScript");
        _gameScript = gameScriptObject.GetComponent<GameScript>();

        var logPosition = new Vector3(0, _gameScript.gameState.ScreenHeight / 2 - _gameScript.gameState.LogHeight / 2 - _gameScript.gameState.KunaiHeight, 0);
        var initStuckKunaiPosition = logPosition;
        initStuckKunaiPosition.Set(initStuckKunaiPosition.x, initStuckKunaiPosition.y - _gameScript.gameState.LogHeight / 2 - _gameScript.gameState.KunaiHeight / 2 + StaticVariables.KunaiMargin, initStuckKunaiPosition.z);

        var initedLogObject = Instantiate(logObject, logPosition, Quaternion.identity);
        initedLogObject.name = StaticVariables.LogObjectName;
        initedLogObject.transform.parent = transform;
        initedLogObject.transform.localScale -= new Vector3(_gameScript.gameState.Scale, _gameScript.gameState.Scale, _gameScript.gameState.Scale);
        var random = new System.Random();
        var initKunaisCount = random.Next(StaticVariables.MaxInitStuck + 1);
        var owo = ((float)random.Next(99) + 1) / 100 ;
        var initAppleCount = appleScriptableObject.SpawnChance > owo ? 1 : 0;
        var totalInitSpawns = initKunaisCount + initAppleCount;
        var spawnPositions = new int[totalInitSpawns];
        var marginForIntersect = 5;

        for (var i = 0; i < totalInitSpawns; i++)
        {
            var angleStart = i * 360 / (totalInitSpawns);
            var angleFin = (i + 1) * 360 / (totalInitSpawns);
            var angle = random.Next(angleFin - 1) - angleStart;
            angle = Mathf.Max(angle, angleStart + marginForIntersect);
            angle = Mathf.Min(angle, angleFin - marginForIntersect);
            spawnPositions[i] = angle;
        }

        for (var i = 0; i < initKunaisCount; i++)
        {
            var initStuckKunai = CreateKunai(initStuckKunaiPosition);
            initStuckKunai.transform.RotateAround(initedLogObject.transform.position, initStuckKunai.transform.forward, spawnPositions[i]);
            initStuckKunai.SendMessage("Stuck", initedLogObject.transform);
        }
        if (initAppleCount == 1)
        {
            var initApplePosition = logPosition;
            var appleHeight = appleObject.GetComponent<SpriteRenderer>().bounds.size.y * (_gameScript.gameState.Scale + 1);
            initApplePosition.Set(initApplePosition.x, initApplePosition.y - _gameScript.gameState.LogHeight / 2 - appleHeight, initApplePosition.z);
            var angle = spawnPositions[totalInitSpawns - 1];
            _spawnedApple = Instantiate(appleObject, initApplePosition, Quaternion.identity);
            _spawnedApple.transform.Rotate(new Vector3(0, 0, 180));
            _spawnedApple.name = StaticVariables.AppleObjectName;
            _spawnedApple.transform.parent = initedLogObject.transform;
            _spawnedApple.transform.localScale -= new Vector3(_gameScript.gameState.Scale, _gameScript.gameState.Scale, _gameScript.gameState.Scale);
            _spawnedApple.transform.RotateAround(initedLogObject.transform.position, _spawnedApple.transform.forward, angle);
        }
        CreateKunai(_gameScript.gameState.StartKunaiPosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateKunai(_gameScript.gameState.StartKunaiPosition);
        }
    }

    public void HitApple()
    {
        if (_spawnedApple != null)
        {
            Destroy(_spawnedApple);
            _spawnedApple = null;
        }
        if (PlayerPrefs.HasKey(StaticVariables.PlayerPrefApple))
        {
            PlayerPrefs.SetInt(StaticVariables.PlayerPrefApple, PlayerPrefs.GetInt(StaticVariables.PlayerPrefApple) + 1);
        }
        else
        {
            PlayerPrefs.SetInt(StaticVariables.PlayerPrefApple, 1);
        }
    }

    public void Stuck()
    {
        _stuckKunai++;
        if (_stuckKunai == StaticVariables.StuckToWin)
        {
            _gameScript.StartLevel(true);
        }
    }

    public void Missed()
    {
        _gameScript.StartLevel(false);
    }

    private GameObject CreateKunai(Vector3 startPosition)
    {
        var resultObject = Instantiate(kunaiObject, startPosition, Quaternion.identity);
        resultObject.name = StaticVariables.GetKunaiName(_kunaiId);
        resultObject.transform.parent = transform;
        resultObject.transform.localScale -= new Vector3(_gameScript.gameState.Scale, _gameScript.gameState.Scale, _gameScript.gameState.Scale);
        _kunaiId++;
        return resultObject;
    }
}
