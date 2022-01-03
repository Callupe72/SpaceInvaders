using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameActionChangeText : AGameAction
{
    public string text;
    public TextMeshProUGUI textToChange;
    private bool _isCompleted = false;

    protected override void OnExecute()
    {
        _isCompleted = false;
        textToChange.text = text;
    }

    public override void ActionUpdate()
    {
            _isCompleted = true;
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
        return $"Change Text to {text}";
    }
}
