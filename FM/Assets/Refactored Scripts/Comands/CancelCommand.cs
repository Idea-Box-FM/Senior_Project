//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CancelCommand : Command
//{
//    LevelEditingManagerRedo levelEditorManager;
//    Command lastCommand;

//    public CancelCommand(bool instantInvoke = true) : base(instantInvoke)
//    {

//        levelEditorManager = LevelEditingManagerRedo.levelEditingManager;
//        lastCommand = levelEditorManager.LastCommand;

//        //base.reprovokeLastCommand = lastCommand.reprovokeLastCommand;
//        //base.isCancelable = lastCommand.isCancelable;
//        //base.requiresCancelation = lastCommand.requiresCancelation;
        
//        if (instantInvoke)
//        {
//            levelEditorManager.Do(this);
//        }
//    }

//    public override ReturnStatus Execute()
//    {
//        //if (!lastCommand.isCancelable)
//        //{
//        //    return ReturnStatus.Failed;
//        //}

//        return lastCommand.Undo();
//    }

//    public override ReturnStatus Undo()
//    {
//        return lastCommand.Execute();
//    }
//}
