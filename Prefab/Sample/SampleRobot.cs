using Godot;
using System;

[Tool]
public partial class SampleRobot : Node3D, IMechanism<(float a, float b, float c), Vector3>
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

    public (float a, float b, float c) generalized = (0, 0, 0);
    [Export]
    public Node3D column;

    [Export]
    public Node3D shoulder;

    [Export]
    public Node3D shoulderConnector;

    [Export]
    public Node3D shoulderConnector2;

    [Export]
    public Node3D elbowConnector;

    [Export]
    public Node3D connectorMover;

    [Export]
    public Node3D forearm;

    [Export]
    public Node3D wrist;

    [Export]
    public Node3D forearmConnector;

    public override void _Process(double delta)
    {
        UpdateGeneralized();
    }

    private void UpdateGeneralized()
    {
        column.RotationDegrees = new Vector3(0, 0, generalized.a);
        shoulder.RotationDegrees = new Vector3(0, generalized.b, 0);
        shoulderConnector.RotationDegrees = new Vector3(0, generalized.b, 0);
        elbowConnector.RotationDegrees = new Vector3(0, -generalized.b, 0);

        connectorMover.RotationDegrees = new Vector3(0, generalized.c, 0);
        forearm.RotationDegrees = new Vector3(0, generalized.c - generalized.b, 0);
        shoulderConnector2.RotationDegrees = new Vector3(0, generalized.b - generalized.c, 0);

        wrist.RotationDegrees = new Vector3(0, -generalized.c, 0);
        forearmConnector.RotationDegrees = new Vector3(0, generalized.c, 0);
    }

    public Vector3 CurrentPosition()
    {
        return SolveForward(generalized) ?? Vector3.Zero;
    }

    public Vector3? SolveForward((float a, float b, float c)gen)
    {
        var a = Mathf.DegToRad(gen.a);
        var b = Mathf.DegToRad(gen.b);
        var c = Mathf.DegToRad(gen.c);
        var column = new Transform3D(new Basis(new Vector3(0, 0, 1), a), Vector3.Zero);
        var shoulder = new Transform3D(new Basis(new Vector3(0, 1, 0), b), new Vector3(0, 0, 0.2f));
        var elbow = new Transform3D(new Basis(new Vector3(0, 1, 0), c - b), new Vector3(0, 0, 1));
        var wrist = new Transform3D(Basis.Identity, new Vector3(1, 0, 0));
        return (column * shoulder * elbow * wrist).Origin;
    }

    public Vector3? SetForward((float a, float b, float c)gen)
    {
        generalized = gen;
        UpdateGeneralized();
        return SolveForward(generalized);
    }

    public (float a, float b, float c) CurrentGeneralized()
    {
        return generalized;
    }

    public (float a, float b, float c)? SolveInverse(Vector3 position)
    {
        var wrist = position - new Vector3(0, 0, 0.2f);
        var dSquared = wrist.LengthSquared();
        if (dSquared > 4)
        {
            return null;
        }
        var alpha = Mathf.Atan2(wrist.Y, wrist.X);
        var beta = Mathf.Atan2(Mathf.Sqrt(wrist.X * wrist.X + wrist.Y * wrist.Y), wrist.Z) -
                   Mathf.Acos(dSquared / (2 * Mathf.Sqrt(dSquared)));
        var gamma = -Mathf.Pi / 2 + Mathf.Acos((2 - dSquared) / 2);

        return (Mathf.RadToDeg(alpha), Mathf.RadToDeg(beta), Mathf.RadToDeg(beta - gamma));
    }

    public (float a, float b, float c)? SetInverse(Vector3 position)
    {
        var solution = SolveInverse(position);
        if (solution is null)
        {
            return null;
        }
        generalized = solution.Value;
        UpdateGeneralized();
        return solution;
    }

    public (float a, float b, float c) DriveSpeed()
    {
        return (45, 90, 90);
    }
}
