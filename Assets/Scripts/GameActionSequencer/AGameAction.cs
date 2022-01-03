using UnityEngine;

public abstract class AGameAction : MonoBehaviour
{
    private bool _isRunning = false;
    public bool IsRunning()
    {
        return _isRunning;
    }

    public void Execute()
    {
        if (_isRunning) return;
        _isRunning = true;
        OnExecute();
    }

    protected abstract void OnExecute();

    public virtual void ActionUpdate() { }

    public virtual bool IsFinished()
    {
        return true;
    }

    public virtual void Reload()
    {
        _isRunning = false;
    }

    void OnValidate()
    {
        Rename();
    }

    public void Rename()
    {
        string newGameObjectName = BuildGameObjectName();
        if (gameObject.name != newGameObjectName)
        {
            gameObject.name = newGameObjectName;
        }
    }

    protected abstract string BuildGameObjectName();
}
