using Godot;

public partial class SampleController : Node
{
    private Controller<IMechanism<(float a, float b, float c), Vector3>> controller;

    [Export]
    public SampleRobot robot;

    public override void _Ready()
    {
        controller = new(robot);

        InitCommands();
    }

    private void InitCommands()
    {
        controller.EnqueueCommand(new Linear3<(float a, float b, float c)>(new Vector3(1, 0, 0.2f), 0.25f));
        controller.EnqueueCommand(new Linear3<(float a, float b, float c)>(new Vector3(1, 1, 00.2f), 0.5f));
        controller.EnqueueCommand(new Linear3<(float a, float b, float c)>(new Vector3(1, 1, 1), 0.75f));
        controller.EnqueueCommand(new Linear3<(float a, float b, float c)>(new Vector3(1, 0, 1), 1.0f));

        controller.EnqueueCommand(new Joint3<Vector3>(new Vector3(1, 0, 0.2f), 0.25f));
        controller.EnqueueCommand(new Joint3<Vector3>(new Vector3(1, -1, 0.2f), 0.5f));
        controller.EnqueueCommand(new Joint3<Vector3>(new Vector3(1, -1, 1), 0.75f));
        controller.EnqueueCommand(new Joint3<Vector3>(new Vector3(1, 0, 1), 1.0f));
    }

    public override void _Process(double delta)
    {
        if (controller is not null)
        {
            var status = controller.Step((float)delta);
            if (status == Status.Error)
            {
                GD.Print("Encountered error!");
                controller = null;
            }
        }
    }
}
