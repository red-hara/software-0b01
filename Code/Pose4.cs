using Godot;

public struct Pose4
{
    public Vector3 translation;
    public float rotation;

    public Pose4(Vector3 translation, float rotation)
    {
        this.translation = translation;
        this.rotation = rotation;
    }
}
