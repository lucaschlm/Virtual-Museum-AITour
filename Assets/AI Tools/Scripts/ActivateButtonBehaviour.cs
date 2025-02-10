using Unity.VisualScripting;
using UnityEngine;

public class ActivateButtonBehaviour : MonoBehaviour
{
    private bool m_mic_on = false;

    public void OnButtonClick()
    {
        SwitchState();

        Debug.Log("Etat du bouton : " +  m_mic_on);

        if (!m_mic_on)
        {
            EventManager.Instance.TriggerRequestSended();
        }
    }

    public void SwitchState()
    {
        m_mic_on = !m_mic_on;
    }
}
