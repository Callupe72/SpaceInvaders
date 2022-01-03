using UnityEngine;

public class GameActionSetActive : AGameAction
{
    public GameObject toSetActive;
    public bool setActive;
    private bool _isCompleted;
    string buildName;


    protected override void OnExecute()
    {
        _isCompleted = false;
     
        if (setActive)
        {
            toSetActive.SetActive(true);
            _isCompleted = true;
        }
        else if(!setActive)
        {
            toSetActive.SetActive(false);
            _isCompleted = true;
        }
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        if (setActive)
        {
            buildName =  $"Set Active {toSetActive.name} to true";
        }
        else if (!setActive)
        {
            buildName = $"Set Active {toSetActive.name} to false";
        }

        return buildName;
    }

    public override void ActionUpdate()
    {

    }
}
