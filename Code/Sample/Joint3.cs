using System;
using Godot;

public class Joint3<Pos> : ICommand<IMechanism<(float a, float b, float c), Pos>>
    where Pos : struct
{
    private Pos start;
    private readonly Pos target;
    private readonly float speed;
    private float progress;

    public Joint3(Pos target, float speed)
    {
        if (speed <= 0 || speed > 1)
        {
            throw new ArgumentException("Speed can't be less or equal to zero or greater than 1");
        }
        this.target = target;
        this.speed = speed;
    }

    public void Init(Controller<IMechanism<(float a, float b, float c), Pos>> controller)
    {
        start = controller.Mechanism.CurrentPosition();
    }

    public Progress Step(Controller<IMechanism<(float a, float b, float c), Pos>> controller, float delta)
    {
        var solutionStart = controller.Mechanism.SolveInverse(start);
        var solutionTarget = controller.Mechanism.SolveInverse(target);

        if (solutionStart is null || solutionTarget is null)
        {
            return Progress.Error;
        }

        var (a, b, c) = controller.Mechanism.DriveSpeed();

        var deltaA = solutionTarget.Value.a - solutionStart.Value.a;
        var deltaB = solutionTarget.Value.b - solutionStart.Value.b;
        var deltaC = solutionTarget.Value.c - solutionStart.Value.c;
        var timeA = Math.Abs(deltaA) / a / speed;
        var timeB = Math.Abs(deltaB) / b / speed;
        var timeC = Math.Abs(deltaC) / c / speed;
        var maximum = Mathf.Max(Mathf.Max(timeA, timeB), timeC);

        if (maximum <= 0)
        {
            return Progress.Done;
        }

        progress += delta / maximum;

        if (progress >= 1)
        {
            if (controller.Mechanism.SetForward(solutionTarget.Value) is null)
            {
                return Progress.Error;
            }
            return Progress.Done;
        }

        var currentA = solutionStart.Value.a + deltaA * progress;
        var currentB = solutionStart.Value.b + deltaB * progress;
        var currentC = solutionStart.Value.c + deltaC * progress;

        if (controller.Mechanism.SetForward((currentA, currentB, currentC)) is null)
        {
            return Progress.Error;
        }
        return Progress.Ongoing;
    }
}