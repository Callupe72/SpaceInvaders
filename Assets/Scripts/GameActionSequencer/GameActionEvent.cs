using UnityEngine;
using UnityEngine.Events;

public class GameActionEvent : AGameAction
{
    public UnityEvent eventToLaunch;

    private bool _isCompleted = false;

    protected override void OnExecute()
    {
        eventToLaunch.Invoke();
        _isCompleted = true;
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        string name = "";

        if (eventToLaunch != null)
        {
            name = $"Launch Event {eventToLaunch.GetPersistentMethodName(0)}";
        }

        else
        {
            name = "Launch Event null";
        }

        return name;
    }
}
