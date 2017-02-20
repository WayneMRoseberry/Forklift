using UnityEngine;

public class carscript : MonoBehaviour {

    private const float forkincrement = 0.005f;

    private bool vehicleEnabled = false;

    public GameObject forkAssembly;
    public Transform forkAssemblyCollider;
    public WheelCollider wheelColliderR;
    public WheelCollider wheelColliderRearR;
    public WheelCollider wheelColliderL;
    public WheelCollider wheelColliderRearL;
    public GameObject wheelfr;
    public GameObject wheelrr;
    public GameObject wheelfl;
    public GameObject wheelrl;
    public GameObject steeringWheel;
    public GameObject playerView;
    public AudioSource liftaudio;
    public float wheelSpeed;
    public float maxForkPosition = 1.0f;
    public float minForkPosition = -0.468f;
    public float maxForkForward = 0.2f;
    public float minForkForward = -0.2f;


    // Update is called once per frame
    void Update()
    {
        if (!vehicleEnabled)
        {
            return;
        }
        DoTheFork();
        DoTheSteering();
        turnHead();
    }

    public bool VehicleEnabled
        {
        get { return this.vehicleEnabled;}
        set {this.vehicleEnabled = value;}
        }

    #region steering and movement
    private void DoTheSteering()
    {
        float t = MoveTheWheels();
        TurnSteeringWheel(t);
        UpdateWheelMeshRotation();
        ApplyBrake();
    }

    private float MoveTheWheels()
    {
        float v = Input.GetAxis("Vertical") * wheelSpeed;

        float t = Input.GetAxis("Horizontal") * wheelSpeed;
        MoveWheel(wheelColliderR, v, t);
        MoveWheel(wheelColliderL, v, t);
        return t;
    }

    private void TurnSteeringWheel(float t)
    {
        Quaternion steerRot = steeringWheel.transform.localRotation;
        steerRot.eulerAngles = new Vector3(0.0f, t * 3, 0.0f);
        steeringWheel.transform.localRotation = steerRot;
    }

    private void UpdateWheelMeshRotation()
    {
        MoveSingleWheel(wheelColliderR, wheelfr);
        MoveSingleWheel(wheelColliderRearR, wheelrr);
        MoveSingleWheel(wheelColliderL, wheelfl);
        MoveSingleWheel(wheelColliderRearL, wheelrl);
    }

    private void MoveSingleWheel(WheelCollider collider, GameObject wheel)
    {
        Quaternion quat;
        Vector3 vect;

        collider.GetWorldPose(out vect, out quat);
        wheel.transform.rotation = quat;
        wheel.transform.position = vect;

    }

    private void MoveWheel(WheelCollider wheel, float velocity, float turn)
    {
        wheel.motorTorque = velocity;
        wheel.steerAngle = turn * 2;
    }

    private const float brakePower = 100;
    private void ApplyBrake()
    {
        float strength = Input.GetAxis("Fire3");
        ApplyBrake(strength);
    }

    public void ApplyBrake(float strength)
    {
        float bForce = strength * brakePower;
        wheelColliderR.brakeTorque = bForce;
        wheelColliderL.brakeTorque = bForce;
    }
    #endregion

    #region fork control

    private void DoTheFork()
    {
        RaiseLowerFork();
        ForkForwardBack();
    }

    private void RaiseLowerFork()
    {

        float dir = 0.0f;
        Vector3 currentforkpos = forkAssembly.transform.localPosition;
        if (Input.GetButton("Fire1"))
        {
            if (currentforkpos.y <= maxForkPosition)
            {
                dir = 1.0f;
            }
        }
        if (Input.GetButton("Fire2"))
        {
            if (currentforkpos.y > minForkPosition)
            {
                dir = -1.0f;
            }
        }
        currentforkpos.y = currentforkpos.y + (forkincrement * dir);
        forkAssembly.transform.localPosition = currentforkpos;
    }

    private void ForkForwardBack()
    {
        Vector3 v = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.Z) && forkAssembly.transform.rotation.x < maxForkForward)
        {
            v.x = 5.0f;

        }
        if (Input.GetKeyDown(KeyCode.X)&& forkAssembly.transform.rotation.x > minForkForward)
        {
            v.x = -5.0f;
        }

        forkAssembly.transform.Rotate(v);
    }
    #endregion

    #region visuals

    private void turnHead()
    {
        // turn head back and forth
        Quaternion quat = playerView.transform.localRotation;
        if (Input.GetKeyDown(KeyCode.A))
        {
            quat.eulerAngles = new Vector3(quat.eulerAngles.x, -120.0f, quat.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            quat.eulerAngles = new Vector3(quat.eulerAngles.x, 0.0f, quat.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            quat.eulerAngles = new Vector3(quat.eulerAngles.x, 120.0f, quat.eulerAngles.z);
        }
        playerView.transform.localRotation = quat;

        // peering side to side
        Vector3 vect = playerView.transform.localPosition;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            vect.x = -0.5f;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            vect.x = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            vect.x = 0.5f;
        }
        playerView.transform.localPosition = vect;
    }

    #endregion
}
