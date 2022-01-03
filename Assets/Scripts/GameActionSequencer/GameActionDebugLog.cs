using UnityEngine;

public class GameActionDebugLog : AGameAction
{
    [TextArea]
    public string message = "";

    protected override void OnExecute()
    {
        Debug.Log(message);
    }

    protected override string BuildGameObjectName()
    {
        return $"Debug : ({message})";
    }
}
