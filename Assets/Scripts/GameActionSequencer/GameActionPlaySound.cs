using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionPlaySound : AGameAction
{
    public string audioName;
    private bool _isCompleted = false;

    protected override void OnExecute()
    {
        _isCompleted = false;
        //AudioManager.instance.PlaySfx(audioName);
    }

    public override void ActionUpdate()
    {
        //if (!AudioManager.instance.sfxSource.isPlaying)
        //{
            _isCompleted = true;
        //}
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }
    public override void Reload()
    {
        base.Reload();
        _isCompleted = false;
    }

    protected override string BuildGameObjectName()
    {
        return "Play Sound " + audioName;
    }
}
