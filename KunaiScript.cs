using UnityEngine;

public class KunaiScript : MonoBehaviour
{
    private State _state = State.Idle;
    private GameObject _logObject;
    private GameObject _gameScript;

    void Start()
    {
        _logObject = GameObject.Find(StaticVariables.LogObjectName);
        _gameScript = GameObject.Find("GameScript");
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
                if (transform.GetComponent<PolygonCollider2D>().bounds.Intersects(_logObject.GetComponent<CircleCollider2D>().bounds))
                {
                    Stuck(_logObject.transform);
                    _gameScript.SendMessage("Stuck");
                    var desiredPosition = _logObject.transform.position.y - _logObject.GetComponent<CircleCollider2D>().bounds.size.y / 2 - GetComponent<PolygonCollider2D>().bounds.size.y / 2 + StaticVariables.KunaiMargin;
                    transform.Translate(0, desiredPosition - transform.position.y, 0);
                }
                else
                {
                    var missed = false;
                    var kunaiId = 0;
                    var stuckKunai = _logObject.transform.Find(StaticVariables.GetKunaiName(kunaiId));
                    while(stuckKunai != null)
                    {
                        missed = missed || stuckKunai.GetComponent<PolygonCollider2D>().bounds.Intersects(transform.GetComponent<PolygonCollider2D>().bounds);
                        kunaiId++;
                        stuckKunai = _logObject.transform.Find(StaticVariables.GetKunaiName(kunaiId));
                    }
                    if (missed)
                    {
                        _gameScript.SendMessage("Missed");
                    }
                    else
                    {
                        transform.Translate(0, 0.1f, 0);
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

    private enum State
    {
        Idle,
        Flying,
        Stuck
    }
}
