using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CopyTool : MonkeyWithButton
{
    public static CopyTool copyTool;

    public List<XML> copiedObject = new List<XML>();

    private void Awake()
    {
        SingletonPattern<CopyTool>(ref copyTool, this);
    }

    public override Command MakeCommand(params object[] parameters)
    {
        FMInfo[] objects = SelectionTool.SelectedObjects;
        List<XML> copyInfo = objects.ToList().Select(x => x.basePrefab.ConvertToXML(x.gameObject)).ToList();

        return new CopyCommand(copyInfo);
    }

    public List<XML> Copy(List<XML> copiedObjects)
    {
        List<XML> temp = copiedObject;
        this.copiedObject = copiedObjects;

        return temp;
    }

    public List<XML> UnCopy(List<XML> copiedObjects)
    {
        List<XML> temp = copiedObject;
        this.copiedObject = copiedObjects;

        return temp;
    }

    public override void Invoke()
    {
        Command command = MakeCommand();

        //switch (SelectionTool.SelectedObjects.Length){
        //    case 0:
        //        break;
        //    case 1:
        //        command = MakeCommand();
        //        break;
        //    default:
        //        MakeMultiStepCommands()
        //}

        levelEditingManager.Execute(command);
    }

    public override bool ActivationCheck(Command command, Status status)
    {
        return SelectionTool.SelectedObjects.Length > 0;
    }
}