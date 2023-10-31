using System;
using Godot;

public class Linear3<Gen> : ICommand<IMechanism<Gen, Vector3>>
    where Gen : struct
{
    private Vector3 start;
    private Vector3 target;
    private readonly float velocity;
    private float progress;

    public Linear3(Vector3 target, float velocity)
    {
        if (velocity <= 0)
        {
            throw new ArgumentException("Velocity can't be less or equal to zero");
        }
        this.target = target;
        this.velocity = velocity;
    }

    public void Init(Controller<IMechanism<Gen, Vector3>> controller)
    {
        start = controller.Mechanism.CurrentPosition();
    }

    public Progress Step(Controller<IMechanism<Gen, Vector3>> controller, float delta)
    {
        var deltaVector = target - start;
        float length = deltaVector.Length();
        if (length <= 0)
        {
            return Progress.Done;
        }

        progress += velocity / length * delta;

        if (progress >= 1)
        {
            if (controller.Mechanism.SetInverse(target) is null)
            {
                return Progress.Error;
            }
            return Progress.Done;
        }

        var current = start.Lerp(target, progress);
        if (controller.Mechanism.SetInverse(current) is null)
        {
            return Progress.Error;
        }
        return Progress.Ongoing;
    }
}
