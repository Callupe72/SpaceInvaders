using UnityEngine;

public class GameActionWait : AGameAction
{
    public float duration = 3f;
    private float _timer = 0f;
    private bool _isCompleted = false;

    protected override void OnExecute()
    {
        _isCompleted = false;
        _timer = 0f;
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        return "Wait " + duration.ToString("F2") + "s";
    }

    public override void ActionUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= duration) {
            _isCompleted = true;
        }
    }

}
