using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandLocation : MonoBehaviour
{
    public bool showController;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerModels;
    public GameObject handPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHand;
    private Animator handAnimator;


    //modeled off VR intergration tutorials and offical XR documentation


    // Start is called before the first frame update
    void Start()
    {
        tryInitialise();
    }

    // function that detects VR device type to check compatability.
    void tryInitialise()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + " " + item.manufacturer + " " + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerModels.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find model");
                spawnedController = Instantiate(controllerModels[0], transform);
            }

            spawnedHand = Instantiate(handPrefab, transform);
            handAnimator = spawnedHand.GetComponent<Animator>();
        }
    }

    //control animator an animations that triggers hand movements on button presses
    void updateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }


        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }



    // Update is called once per frame
    //first part checks to see if VR devices connected are valid to use. If fail, continues to look for valie device.
    //if success, set the models used for hand.
    void Update()
    {
        if(!targetDevice.isValid )
        {
            tryInitialise();
        }
        else
        {
            if (showController)
            {
                spawnedHand.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHand.SetActive(true);
                spawnedController.SetActive(false);
                updateHandAnimation();
            }
        }
    }
}
