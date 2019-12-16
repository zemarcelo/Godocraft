using Godot;
using System;

public class MoveCamera : Camera
{
    [Export] readonly float moveSpeed = 10f;

    //View variables
    [Export] readonly float mouseSensitivity = .3f;
    [Export] readonly float mouseSpeed = 10f;
    //[Export] readonly float anguloMaximo = 90.0f;
    //[Export] readonly float anguloMinimo = -90.0f;
    bool altIsPressed = false;


    float cameraVerticalAngle = 0;
    float cameraHorizontalAngle = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcessInput(true);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_right"))
        {
            TranslateObjectLocal(Transform.basis.x * moveSpeed * delta);
        }

        if (Input.IsActionPressed("ui_left"))
        {
            TranslateObjectLocal(-Transform.basis.x * moveSpeed * delta);
        }

        if (Input.IsActionPressed("ui_up"))
        {
            TranslateObjectLocal(-Transform.basis.z * moveSpeed * delta);
        }

        if (Input.IsActionPressed("ui_down"))
        {
            TranslateObjectLocal(Transform.basis.z * moveSpeed * delta);
        }

        if(altIsPressed)
        {


            RotateY(Mathf.Deg2Rad(cameraHorizontalAngle) * mouseSpeed * delta);
            RotateX(Mathf.Deg2Rad(cameraVerticalAngle) * mouseSpeed * delta);
        }


        Transform.Orthonormalized();

    }
    public override void _Input(InputEvent @event)
    {
        UpdateCameraInput(@event);
    }

    void UpdateCameraInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Alt)
                altIsPressed = true;
            else
                altIsPressed = false;

        if (@event is InputEventMouseMotion mouseMotion)
        {

            cameraHorizontalAngle = -mouseMotion.Relative.x * mouseSensitivity;
            cameraVerticalAngle   = -mouseMotion.Relative.y * mouseSensitivity;


        } 

    }

}
