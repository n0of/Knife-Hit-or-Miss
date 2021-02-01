using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    public GameObject kunaiObject;
    public GameObject logObject;

    private Vector3 _startKunaiPosition = new Vector3();
    private float _scale = 1;
    private int _stuckKunai = 0;
    private int _kunaiId = 0;

    void Start()
    {
        var topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        var height = bottomRight.y - topLeft.y;
        var width = bottomRight.x - topLeft.x;
        var logHeight = logObject.GetComponent<SpriteRenderer>().bounds.size.y;
        var kunaiHeight = kunaiObject.GetComponent<SpriteRenderer>().bounds.size.y;
        _scale = Mathf.Min(width / (logHeight + 2 * kunaiHeight), 1);
        logHeight *= _scale;
        kunaiHeight *= _scale;
        _scale -= 1;

        var logPosition = new Vector3(0, height / 2 - logHeight / 2 - kunaiHeight, 0);
        var initStuckKunaiPosition = logPosition;
        initStuckKunaiPosition.Set(initStuckKunaiPosition.x, initStuckKunaiPosition.y - logHeight / 2 - kunaiHeight / 2 + StaticVariables.KunaiMargin, initStuckKunaiPosition.z);
        _startKunaiPosition = new Vector3(0, -height / 2 + kunaiHeight / 2 + StaticVariables.KunaiMargin, 0);

        var initedLogObject = Instantiate(logObject, logPosition, Quaternion.identity);
        initedLogObject.name = StaticVariables.LogObjectName;
        initedLogObject.transform.localScale -= new Vector3(_scale, _scale, _scale);
        var random = new System.Random();
        for (var i = 0; i < random.Next(StaticVariables.MaxInitStuck + 1); i++)
        {
            var initStuckKunai = CreateKunai(initStuckKunaiPosition);
            initStuckKunai.transform.RotateAround(initedLogObject.transform.position, initStuckKunai.transform.forward, random.Next(359));
            initStuckKunai.SendMessage("Stuck", initedLogObject.transform);
        }
        CreateKunai(_startKunaiPosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateKunai(_startKunaiPosition);
        }
    }

    public void Stuck()
    {
        _stuckKunai++;
        if (_stuckKunai == StaticVariables.StuckToWin)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void Missed()
    {
        SceneManager.LoadScene("Game");
    }

    private GameObject CreateKunai(Vector3 startPosition)
    {
        var resultObject = Instantiate(kunaiObject, startPosition, Quaternion.identity);
        resultObject.name = StaticVariables.GetKunaiName(_kunaiId);
        resultObject.transform.localScale -= new Vector3(_scale, _scale, _scale);
        _kunaiId++;
        return resultObject;
    }
}
