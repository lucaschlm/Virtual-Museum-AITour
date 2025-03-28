/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Meta.WitAi.Dictation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Meta.Voice.Samples.Dictation
{
    public class DictationActivation : MonoBehaviour
    {
        [FormerlySerializedAs("dictation")]
        [SerializeField] private DictationService _dictation;

        [SerializeField]
        private InputActionReference m_activateMic;

        [SerializeField]
        private AudioSource m_micOn;

        [SerializeField]
        private AudioSource m_micOff;

        [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
        [SerializeField]
        private string m_fullTranscription = "";

        public void OnEnable()
        {


            if (m_micOn == null)
            {
                Debug.Log("[DictationActivation] : Il manque un 'm_micOn'.");
            }

            if (m_micOff == null)
            {
                Debug.Log("[DictationActivation] : Il manque un 'm_micOff'.");
            }


            if (m_activateMic == null)
            {
                Debug.Log("[DictationActivation] : Il manque un 'InputActionReference'.");
            }
            else
            {
                m_activateMic.action.performed += ToggleActivation;
                m_activateMic.action.canceled += ToggleDesactivation;

                _dictation.DictationEvents.OnFullTranscription.AddListener(HandleFullTranscription);
            }
        }



        public void ToggleActivation(InputAction.CallbackContext context)
        {
            if (!_dictation.MicActive)
            {
                EventManager.Instance.TriggerDictationStarted();
                m_fullTranscription = "";
                PlayMicStartSound();
                //Debug.Log("[DictationActivation] D�but de l'enregistrement...");
                _dictation.Activate();
            }
        }



        public void ToggleDesactivation(InputAction.CallbackContext context)
        {
            if (_dictation.MicActive)
            {
                EventManager.Instance.TriggerDictationEnded();
                PlayMicStopSound();
                //Debug.Log("[DictationActivation] ...Fin de l'enregistrement");
                _dictation.Deactivate();
            }
        }

        public void HandleFullTranscription(string transcription)
        {
            m_fullTranscription += transcription + " ";

            if (!_dictation.MicActive)
            {
                //Debug.Log("Vous : " + m_fullTranscription);

                if (m_fullTranscription != " ")
                {
                    EventManager.Instance.TriggerOnAddedToPrompt(m_fullTranscription);
                    EventManager.Instance.TriggerRequestSended();
                }
            }
        }


        void PlayMicStartSound()
        {
            if (m_micOn != null)
            {
                m_micOn.Play();
            }
        }

        void PlayMicStopSound()
        {
            if (m_micOff != null)
            {
                m_micOff.Play();
            }
        }



        public void OnDisable()
        {
            m_activateMic.action.performed -= ToggleActivation;
            m_activateMic.action.canceled -= ToggleDesactivation;
            _dictation.DictationEvents.OnFullTranscription.RemoveListener(HandleFullTranscription);
        }
    }
}
