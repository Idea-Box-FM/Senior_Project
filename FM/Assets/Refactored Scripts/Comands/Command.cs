//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////It is important that the order of ReturnStatus be best scenario to worst scenario for MultiStepCommand undo function
//public enum ReturnStatus
//{
//    Success,
//    Canceled,
//    Failed
//}

//public abstract class Command
//{
//    //public bool reprovokeLastCommand;
//    //public bool isCancelable;
//    //public bool requiresCancelation;

//    //object[] parameters;

//    //public delegate ReturnStatus Functions(ref object[] parameters);

//    //Functions doFunction; 
//    //Functions undoFunction;

//    //public Command(bool reprovokeLastCommand, bool isCancelable, bool requiresCancelation)
//    //{
//    //    this.reprovokeLastCommand = reprovokeLastCommand;
//    //    this.isCancelable = isCancelable;
//    //    this.requiresCancelation = requiresCancelation;
//    //}

//    public Command(bool instantInvoke)
//    {
//        if (instantInvoke)
//        {
//            LevelEditingManagerRedo.levelEditingManager.Do(this);
//        }
//    }

//    //protected Command(){}

//    //public Command(Functions doFunction, Functions undoFunction, bool reprovokeLastCommand, params object[] parameters) : this(doFunction, undoFunction, reprovokeLastCommand, true, parameters) { }

//    //public Command(Functions doFunction, Functions undoFunction, bool reprovokeLastCommand, bool instantInvoke = true, params object[] parameters)
//    //{
//    //    this.doFunction = doFunction;
//    //    this.undoFunction = undoFunction;
//    //    this.parameters = parameters;
//    //    this.reprovokeLastCommand = reprovokeLastCommand;

//    //    if (instantInvoke)
//    //    {
//    //        LevelEditingManagerRedo.levelEditingManager.Do(this);
//    //    }
//    //}

//    //public virtual ReturnStatus Do()
//    //{
//    //    return doFunction(ref parameters);
//    //}

//    public abstract ReturnStatus Execute();

//    //public virtual ReturnStatus Undo()
//    //{
//    //ReturnStatus returnStatus = undoFunction(ref parameters);

//    //if (reprovokeLastCommand)
//    //{
//    //    LevelEditingManagerRedo.levelEditingManager.ReprovokeLastCommand();
//    //}

//    //return returnStatus;
//    //}

//    public abstract ReturnStatus Undo();

//    //public T GetParamater<T>(int location)
//    //{
//    //    return (T)parameters[location];
//    //}
//}