using UnityEngine;

public class KunaiScript : MonoBehaviour
{
    private State _state = State.Idle;
    private GameObject _logObject;
    private LevelScript _levelScript;
    private GameState _gameState;
    private Transform _apple;

    void Start()
    {
        _logObject = GameObject.Find(StaticVariables.LogObjectName);
        _levelScript = GameObject.Find(StaticVariables.LevelObjectName).GetComponent<LevelScript>();
        _gameState = GameObject.Find("GameScript").GetComponent<GameScript>().gameState;
        _apple = _logObject != null ? _logObject.transform.Find(StaticVariables.AppleObjectName) : null;
    }

    void Update()
    {
        switch (_state)
        {
            case (State.Idle):
                if (Input.GetMouseButtonDown(0))
                {
                    _state = State.Flying;
                }
                break;
            case (State.Flying):
                if (IsIntersects(_logObject.GetComponent<CircleCollider2D>().bounds))
                {
                    Stuck(_logObject.transform);
                    _levelScript.Stuck();
                    var desiredPosition = _logObject.transform.position.y - _gameState.LogHeight / 2 - _gameState.KunaiHeight / 2 + StaticVariables.KunaiMargin;
                    transform.Translate(0, desiredPosition - transform.position.y, 0);
                }
                else
                {
                    if (_apple != null && IsIntersects(_apple.GetComponent<CircleCollider2D>().bounds))
                    {
                        _levelScript.HitApple();
                    }
                    var missed = false;
                    var kunaiId = 0;
                    var stuckKunai = _logObject.transform.Find(StaticVariables.GetKunaiName(kunaiId));
                    while(stuckKunai != null)
                    {
                        missed = missed || IsIntersects(stuckKunai.GetComponent<PolygonCollider2D>().bounds);
                        kunaiId++;
                        stuckKunai = _logObject.transform.Find(StaticVariables.GetKunaiName(kunaiId));
                    }
                    if (missed)
                    {
                        _levelScript.Missed();
                    }
                    else
                    {
                        transform.Translate(0, 2 * _gameState.ScreenHeight * Time.deltaTime, 0);
                    }
                }
                break;
        }
    }

    public void Stuck(Transform parentTransform)
    {
        _state = State.Stuck;
        transform.parent = parentTransform;
    }

    public void RotateAroundLog(float angle)
    {
        transform.RotateAround(_logObject.transform.position, transform.forward, angle);
    }

    private bool IsIntersects(Bounds bounds)
    {
        return transform.GetComponent<PolygonCollider2D>().bounds.Intersects(bounds);
    }

    private enum State
    {
        Idle,
        Flying,
        Stuck
    }
}
