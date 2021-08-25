using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class locomotionController : MonoBehaviour
{
    public XRController teleportRay;
    public InputHelpers.Button teleportStick;
    public float threshold = 0.1f;

    public XRNode inputSource;
    private XRRig rig;
    public float speed = 1.0f;
    private Vector2 inputAxis;
    private CharacterController character;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        character.enabled = true;
    }

    public bool checkActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportStick, out bool isActivated, threshold);
        character.enabled = !isActivated;
        return isActivated;
    }

    // Update is called once per frame
    //checks to see if necessary XR companants are present and if they are responding to button presses
    void Update()
    {
        if(teleportRay)
        {
            teleportRay.gameObject.SetActive(checkActivated(teleportRay));
        }
        
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    //function called every frame to provide accurate head movements, paired with function below.
    private void FixedUpdate()
    {
        controllerFollowHead();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);
    }

    //function that moves camera to follow head movements.
    void controllerFollowHead()
    {
        character.height = rig.cameraInRigSpaceHeight + 0.2f;
        Vector3 controllerCentre = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(controllerCentre.x, character.height / 2 + character.skinWidth, controllerCentre.z);
    }
}
