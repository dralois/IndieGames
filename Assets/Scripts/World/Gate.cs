﻿using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    //------------------------------------------------------
    //Verweis auf das Fallgatter-GameObject
    //------------------------------------------------------
    [Header("Settings")]
    [SerializeField]
    private GameObject m_Gate;
    //------------------------------------------------------
    //Position im geschlossenen Zustand
    //------------------------------------------------------
    [SerializeField]
    private Vector3 m_Closed;
    //------------------------------------------------------
    //Position im geöffneten Zustand
    //------------------------------------------------------
    [SerializeField]
    private Vector3 m_Opened;
    //------------------------------------------------------
    //Geschwindigkeit mit der sich das Tor öffnet
    //------------------------------------------------------
    [SerializeField]
    private float m_OpenSpeed;
    //------------------------------------------------------
    //Aktueller Torstatus
    //------------------------------------------------------
    private GateStatus m_Current = GateStatus.Close;
    
    /// <summary>
    /// Mögliche Torpositionen
    /// </summary>
    private enum GateStatus
    {
        None,
        Open,
        Close
    }

    /// <summary>
    /// Öffnet das Tor
    /// </summary>
    public void OpenGate()
    {
        //------------------------------------------------------
        //Tor muss geschlossen sein
        //------------------------------------------------------
        if (m_Current == GateStatus.Close)
        {
            //------------------------------------------------------
            //Öffne Tor
            //------------------------------------------------------
            StartCoroutine(MoveGate(GateStatus.Open));
            //------------------------------------------------------
            //Spiele Soundeffekt ab
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayGateOpen();
        }
    }

    /// <summary>
    /// Schließt das Tor
    /// </summary>
    public void CloseGate()
    {
        //------------------------------------------------------
        //Tor muss offen sein
        //------------------------------------------------------
        if (m_Current == GateStatus.Open)
        {
            //------------------------------------------------------
            //Schließe Tor
            //------------------------------------------------------
            StartCoroutine(MoveGate(GateStatus.Close));
            //------------------------------------------------------
            //Spiele Soundeffekt ab
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayGateClose();        
        }
    }

    /// <summary>
    /// Coroutine die das Tor öffnet/schließt
    /// </summary>
    /// <param name="pi_Direction">Bewegungsrichtung</param>    
    private IEnumerator MoveGate(GateStatus pi_Direction)
    {
        //------------------------------------------------------
        //Limitiere Bewegung
        //------------------------------------------------------
        while (pi_Direction == GateStatus.Open ? m_Gate.transform.position.y > m_Opened.y :
                                                 m_Gate.transform.position.y < m_Closed.y)
        {
            //------------------------------------------------------
            //Nur falls nicht im Editmodus
            //------------------------------------------------------
            if (!UIManager.Instance.EditEnabled)
            {
                //------------------------------------------------------
                //Je nach Richtung
                //------------------------------------------------------
                if (pi_Direction == GateStatus.Close)
                {
                    //------------------------------------------------------
                    //Bewege entsprechend
                    //------------------------------------------------------
                    m_Gate.transform.Translate(0, Mathf.Abs(m_OpenSpeed) * Time.deltaTime, 0, Space.World);
                    yield return null;
                }
                else if (pi_Direction == GateStatus.Open)
                {
                    m_Gate.transform.Translate(0, (-1) * Mathf.Abs(m_OpenSpeed) * Time.deltaTime, 0, Space.World);
                    yield return null;
                }
            }
            else
            {
                //------------------------------------------------------
                //Ansonsten warte
                //------------------------------------------------------
                yield return null;
            }
        }
        //------------------------------------------------------
        //Nach Abschluss ändere aktuelle Position
        //------------------------------------------------------
        m_Current = pi_Direction;
    }    
}