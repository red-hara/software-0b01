using Godot;
using System;

[Tool]
public partial class TaskRobot : Node3D, IMechanism<(float a, float b, float c, float d), Pose4>
{
    [Export]
    public float A
    {
        get => generalized.a;
        set => generalized.a = value;
    }
    [Export]
    public float B
    {
        get => generalized.b;
        set => generalized.b = value;
    }
    [Export]
    public float C
    {
        get => generalized.c;
        set => generalized.c = value;
    }
    [Export]
    public float D
    {
        get => generalized.d;
        set => generalized.d = value;
    }

    public (float a, float b, float c, float d) generalized = (0, 0, 0, 0);
    [Export]
    public Node3D column;

    [Export]
    public Node3D shoulder;

    [Export]
    public Node3D forearm;

    [Export]
    public Node3D wrist;

    [Export]
    public Node3D flange;

    [Export]
    public Node3D connector;

    [Export]
    public Node3D wristConnector;

    [Export]
    public Node3D columnConnector;

    [Export]
    public Node3D mover;

    [Export]
    public Node3D forearmConnector;

    public override void _Process(double delta)
    {
        // Fix here.
    }

    public Pose4 CurrentPosition()
    {
        return SolveForward(generalized).Value;
    }

    public Pose4? SolveForward((float a, float b, float c, float d)gen)
    {
        // Fix here.
        throw new NotImplementedException();
    }

    public Pose4? SetForward((float a, float b, float c, float d)generalized)
    {
        this.generalized = generalized;
        return SolveForward(generalized);
    }

    public (float a, float b, float c, float d) CurrentGeneralized()
    {
        return generalized;
    }

    public (float a, float b, float c, float d)? SolveInverse(Pose4 position)
    {
        // Fix here.
        throw new NotImplementedException();
    }

    public (float a, float b, float c, float d)? SetInverse(Pose4 position)
    {
        var solution = SolveInverse(position);
        if (solution is null)
        {
            return null;
        }
        generalized = solution.Value;
        return solution;
    }

    public (float a, float b, float c, float d) DriveSpeed()
    {
        return (130, 130, 130, 30);
    }
}
