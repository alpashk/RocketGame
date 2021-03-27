using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour, IDragHandler,IPointerDownHandler, IPointerUpHandler
{

    public delegate void Rotate(float deltaZ);
    public static event Rotate RotationControl;

    public delegate void Flight(); //false when you stop holding the button
    public static event Flight FlightControl;
    public static event Flight OffThrottle;

    private bool pointerHold = false;
    public void OnDrag(PointerEventData eventData)
    {
        if (RotationControl != null)
        {
            RotationControl(eventData.position.x - eventData.pressPosition.x);
            eventData.pressPosition = eventData.position;
        }
        //перезаписываем точку касания для зануления следующего угла поворота, если игрок не изменит положение пальца
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        pointerHold = true;
        StartCoroutine(HoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerHold = false;
        if (OffThrottle != null)
        {
            OffThrottle();
        }
    }
    IEnumerator HoldRoutine()
    {
        while(pointerHold)
        {
            if (FlightControl != null)
            {
                FlightControl();
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

    }

}
