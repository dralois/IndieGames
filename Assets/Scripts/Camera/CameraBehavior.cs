﻿using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Settings")]
    //------------------------------------------------------
    //Zoomgeschwindigkeit
    //------------------------------------------------------
    [SerializeField]
    private float m_ZoomSpeed = 50f;
    //------------------------------------------------------
    //Maximale/Minimale Kamerahöhe
    //------------------------------------------------------
    [SerializeField]
    private float m_ZoomMin = 0f;
    [SerializeField]
    private float m_ZoomMax = 250f;
    //------------------------------------------------------
    //Kamerageschwindigkeit
    //------------------------------------------------------
    [SerializeField]
    private float m_MoveSpeed = 10f;
    //------------------------------------------------------
    //Kartenmittelpunkt
    //------------------------------------------------------
    [SerializeField]
    private Vector3 m_RotatePoint = new Vector3(0, 0, 0);
    //------------------------------------------------------
    //Touchpunkte
    //------------------------------------------------------
    private Touch m_TouchZero, m_TouchOne;
    private Vector2 m_PrevTouchZero, m_PrevTouchOne;

    private void Update () {
        //------------------------------------------------------
        //Falls es zwei Touches gab
        //------------------------------------------------------
        if (Input.touchCount == 2)
        {
            //------------------------------------------------------
            //Hole Neue
            //------------------------------------------------------
            m_TouchZero = Input.GetTouch(0);
            m_TouchOne = Input.GetTouch(1);
            //------------------------------------------------------
            //Und Alte
            //------------------------------------------------------
            m_PrevTouchZero = m_TouchZero.position - m_TouchZero.deltaPosition;
            m_PrevTouchOne = m_TouchOne.position - m_TouchOne.deltaPosition;
            //------------------------------------------------------
            //Rotiere, Bewege, Zoome
            //------------------------------------------------------
            CameraRotate();
            CameraMove();
            CameraZoom();            
        }
    }

    /// <summary>
    /// Rotiert Kamera
    /// </summary>
    private void CameraRotate() {
        //------------------------------------------------------
        //Bestimme Delta
        //------------------------------------------------------
        float l_TouchZeroDeltaAngle = Mathf.Atan2((m_TouchZero.position - m_TouchOne.position).y,
                                                  (m_TouchZero.position - m_TouchOne.position).x) * Mathf.Rad2Deg;
        float l_TouchOneDeltaAngle =  Mathf.Atan2((m_PrevTouchZero - m_PrevTouchOne).y,
                                                  (m_PrevTouchZero - m_PrevTouchOne).x) * Mathf.Rad2Deg;
        //------------------------------------------------------
        //Rotiere um den Mittelpunkt
        //------------------------------------------------------
        Camera.main.transform.RotateAround(m_RotatePoint, Vector3.up, l_TouchZeroDeltaAngle - l_TouchOneDeltaAngle);
    }

    /// <summary>
    /// Bewegt Kamera
    /// </summary>
    private void CameraMove() {
        //------------------------------------------------------
        //Falls sich beide Finger bewegen
        //------------------------------------------------------
        if (m_TouchZero.phase != TouchPhase.Stationary && m_TouchOne.phase != TouchPhase.Stationary)
        {
            //------------------------------------------------------
            //Berechne Delta
            //------------------------------------------------------
            Vector3 l_CurrTouchAvg = (m_TouchOne.position + m_TouchZero.position) / 2;
            Vector3 l_PrevTouchAvg = (m_PrevTouchOne + m_PrevTouchZero) / 2;
            //------------------------------------------------------
            //Bewege
            //------------------------------------------------------
            Camera.main.transform.Translate((l_PrevTouchAvg - l_CurrTouchAvg) * Time.deltaTime * m_MoveSpeed);
        }
    }

    /// <summary>
    /// Zoomt Kamera rein und raus
    /// </summary>
    private void CameraZoom() {
        //------------------------------------------------------
        //Berechne Distanz zwischen den Touches
        //------------------------------------------------------
        float l_PrevTouchMag = (m_PrevTouchZero - m_PrevTouchOne).magnitude;
        float l_CurrTouchMag = (m_TouchZero.position - m_TouchOne.position).magnitude;
        //------------------------------------------------------
        //Berechne Delta der Distanz
        //------------------------------------------------------
        float l_DeltaMagDiff = l_PrevTouchMag - l_CurrTouchMag;
        //------------------------------------------------------
        //Bestimme neue Position
        //------------------------------------------------------
        Vector3 l_NewPos = Camera.main.transform.position +
            Camera.main.transform.forward * -1 * l_DeltaMagDiff * m_ZoomSpeed * Time.deltaTime;
        //------------------------------------------------------
        //Bewege ggf. auf neue Position
        //------------------------------------------------------
        if (l_NewPos.y <= m_ZoomMax && l_NewPos.y >= m_ZoomMin)
            Camera.main.transform.position = l_NewPos;
    }
}