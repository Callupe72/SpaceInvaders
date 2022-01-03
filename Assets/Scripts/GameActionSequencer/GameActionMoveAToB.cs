using UnityEngine;

public class GameActionMoveAToB : AGameAction
{
    public Transform target;
    public Transform pointB;
    public bool freezeY = true;
    public float speedMove;
    public float sensibility = 0.005f;

    private bool _isCompleted;
    private bool stop = false;
    Vector3 posToGo = Vector3.zero;

    protected override void OnExecute()
    {
        _isCompleted = false;
        if (freezeY)
            posToGo = new Vector3(pointB.position.x, target.position.y, pointB.position.z);
        else
            posToGo = new Vector3(pointB.position.x, pointB.position.y, pointB.position.z);
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        return "Move " + target.name + " to " + pointB.name;
    }

    public override void ActionUpdate()
    {
        if((posToGo - target.position).magnitude >= sensibility)
        {
            Vector3 dir = (posToGo - target.position).normalized;
            target.position += dir * Time.deltaTime * speedMove;
        }
        else
        {
            _isCompleted = true;
        }
    }

    public void EndMovement()
    {
        posToGo = target.position;
    }
}
