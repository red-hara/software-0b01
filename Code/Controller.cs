using System.Collections.Generic;

public class Controller<Mech>
{
    public Mech Mechanism { get; }
    private ICommand<Mech> currentCommand;
    private readonly Queue<ICommand<Mech>> commands;

    public Controller(Mech mechanism)
    {
        Mechanism = mechanism;
        commands = new();
    }

    public Status Step(float delta)
    {
        if (currentCommand is not null)
        {
            Progress progress = currentCommand.Step(this, delta);
            switch (progress)
            {
            case Progress.Ongoing:
                break;
            case Progress.Done:
                currentCommand = null;
                StartNextCommand();
                break;
            case Progress.Error:
                return Status.Error;
            }
        }
        else
        {
            StartNextCommand();
        }
        return Status.Ok;
    }

    private void StartNextCommand()
    {
        if (commands.Count > 0)
        {
            currentCommand = commands.Dequeue();
            currentCommand.Init(this);
        }
    }

    public void EnqueueCommand(ICommand<Mech> command)
    {
        commands.Enqueue(command);
    }
}

public enum Status
{
    Ok,
    Error,
}