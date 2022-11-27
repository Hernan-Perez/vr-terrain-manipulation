using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace VRTerrainManipulation
{
    public class HandMeshManipulation : MonoBehaviour
    {
        public enum Hands { Right = SteamVR_Input_Sources.RightHand, Left = SteamVR_Input_Sources.LeftHand };

        [Header("General")]
        [Tooltip("The forward axis of this transform will be used to aim.")]
        public Transform AimRoot;
        public Hands Hand = Hands.Right;

        [Header("Inflate/Deflate parameters")]
        public SteamVR_Action_Boolean InflateMeshInput;
        public SteamVR_Action_Boolean DeflateMeshInput;
        public float inflateDeflateRadius = 3f;
        public float inflateDeflateStrength = 1f;

        [Header("Smooth parameters")]
        public SteamVR_Action_Boolean SmoothMeshInput;
        public float smoothRadius = 3f;
        public float smoothStrength = 1f;


        void Start()
        {

        }

        void Update()
        {
            RaycastHit hit;
            bool? inflate = null;
            if (InflateMeshInput != null && InflateMeshInput.GetState((SteamVR_Input_Sources)Hand))
            {
                inflate = true;
            }
            else if (DeflateMeshInput != null && DeflateMeshInput.GetState((SteamVR_Input_Sources)Hand))
            {
                inflate = false;
            }

            if (inflate != null)
            {
                if (Physics.Raycast(new Ray(AimRoot.position, AimRoot.forward), out hit, Mathf.Infinity))
                {
                    ManipulableMesh mm = hit.collider.gameObject.GetComponent<ManipulableMesh>();
                    if (mm != null)
                        mm.ModifyMesh(hit.point, inflateDeflateRadius, inflateDeflateStrength * Time.deltaTime, (bool)inflate);
                }
            }

            if (SmoothMeshInput != null && SmoothMeshInput.GetState((SteamVR_Input_Sources)Hand))
            {
                if (Physics.Raycast(new Ray(AimRoot.position, AimRoot.forward), out hit, Mathf.Infinity))
                {
                    ManipulableMesh mm = hit.collider.gameObject.GetComponent<ManipulableMesh>();
                    if (mm != null)
                        mm.SmoothMesh(hit.point, smoothRadius, smoothStrength * Time.deltaTime);
                }
            }
        }
    }
}