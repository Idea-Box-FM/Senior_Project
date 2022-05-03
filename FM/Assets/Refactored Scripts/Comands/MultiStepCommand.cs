public class MultiStepCommand : Command
{
    public Command[] commands;

    public MultiStepCommand(params Command[] commands) : base()
    {
        this.commands = commands;
    }

    public override Status Execute()
    {
        Status Status = Status.Success;
        for (int currentCommand = 0; currentCommand < commands.Length; currentCommand++)
        {
            Command command = commands[currentCommand];

            Status = command.Execute();

            if (Status != Status.Success)
            {
                Undo(currentCommand);
                return Status.Failed;
            }
        }

        return Status;
    }

    public override Status Undo()
    {
        return Undo(commands.Length);
    }

    /// <summary>
    /// This is made to undo a multistep command if one of the commands can not be executed
    /// </summary>
    /// <param name="stopPoint">The last command that does not need to be undone</param>
    /// <returns></returns>
    public Status Undo(int stopPoint)
    {
        Status Status = Status.Success;

        for (int currentCommand = 0; currentCommand != stopPoint; currentCommand++)
        {
            Command command = commands[currentCommand];

            Status newStatus = command.Undo();

            if (Status < newStatus)
            {
                Status = newStatus;
            }
        }

        return Status;
    }
}
