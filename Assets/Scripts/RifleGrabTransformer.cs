using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class RifleGrabTransformer : XRBaseGrabTransformer
{
    public override void Process(XRGrabInteractable interactable, XRInteractionUpdateOrder.UpdatePhase phase, ref Pose targetPose, ref Vector3 localScale)
    {
        // Execute only during the Dynamic phase when movement is calculated
        if (phase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (interactable.interactorsSelecting.Count == 2)
            {
                Transform hand1 = interactable.interactorsSelecting[0].transform;
                Transform hand2 = interactable.interactorsSelecting[1].transform;

                Vector3 aimDirection = hand2.position - hand1.position;

                if (aimDirection.sqrMagnitude > 0.01f)
                {
                    // Overwrite only the rotation. The targetPose.position is left untouched 
                    // because Unity already calculated your PrimaryGrip attach offset perfectly.
                    targetPose.rotation = Quaternion.LookRotation(aimDirection, hand1.up);
                }
            }
        }
    }
}