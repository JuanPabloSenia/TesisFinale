using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PipeCanvas : MonoBehaviour, IPointerDownHandler
{

    Vector3 aux;
    public void OnPointerDown(PointerEventData ped)
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.ScreenToWorldPoint(ped.position), Vector3.forward);
        aux = Camera.main.ScreenToWorldPoint(ped.position);
        if (Physics.Raycast(ray, out hit, 100) && !DialogueController.INSTANCE.onDialogue)
        {
            if (hit.collider.tag == "PipeSlot")
            {
                hit.collider.GetComponent<PipeBehaviour>().RotatePipe();
                PipeController.INSTANCE.connectedPoints = 0;
            }
            if (hit.collider.name == "Done")
            {
                if (!PipeController.INSTANCE.levelFinished) PipeController.INSTANCE.CheckLevel();
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(aux, Vector3.forward * 100);
    }
}
