using System;
using UnityEngine;
using UnityEngine.SceneManagement;



class ReponseManager : MonoBehaviour {

    void Start(){

    }


    void Update(){

    }

    public void DesactiveRen(){
        // Désactive les réponses de toutes les questions de la renaissance
        Debug.Log("Désactive Réponses Renaissance");
        transform.Find("Q1Ren")?.gameObject.SetActive(false);
        transform.Find("Q2Ren")?.gameObject.SetActive(false);
        transform.Find("Q3Ren")?.gameObject.SetActive(false);
        transform.Find("Q4Ren")?.gameObject.SetActive(false);
        transform.Find("Q5Ren")?.gameObject.SetActive(false);
    }

    public void DesactiveImp(){
        Debug.Log("Désactive Réponses Impressionnisme");
        transform.Find("Q1Imp")?.gameObject.SetActive(false);
        transform.Find("Q2Imp")?.gameObject.SetActive(false);
        transform.Find("Q3Imp")?.gameObject.SetActive(false);
        transform.Find("Q4Imp")?.gameObject.SetActive(false);
        transform.Find("Q5Imp")?.gameObject.SetActive(false);
    }

    public void ActiveQ1Ren(){
        Debug.Log("Activation Réponse Q1 Renaissance");
        DesactiveRen();
        transform.Find("Q1Ren")?.gameObject.SetActive(true);
    }

    public void ActiveQ2Ren(){
        Debug.Log("Activation Réponse Q2 Renaissance");
        DesactiveRen();
        transform.Find("Q2Ren")?.gameObject.SetActive(true);
    }

    public void ActiveQ3Ren(){
        Debug.Log("Activation Réponse Q3 Renaissance");
        DesactiveRen();
        transform.Find("Q3Ren")?.gameObject.SetActive(true);
    }

    public void ActiveQ4Ren(){
        Debug.Log("Activation Réponse Q4 Renaissance");
        DesactiveRen();
        transform.Find("Q4Ren")?.gameObject.SetActive(true);
    }

    public void ActiveQ5Ren(){
        Debug.Log("Activation Réponse Q5 Renaissance");
        DesactiveRen();
        transform.Find("Q5Ren")?.gameObject.SetActive(true);
    }

    public void ActiveQ1Imp(){
        Debug.Log("Activation Réponse Q1 Impressionnisme");
        DesactiveImp();
        transform.Find("Q1Imp")?.gameObject.SetActive(true);
    }

    public void ActiveQ2Imp(){
        Debug.Log("Activation Réponse Q2 Impressionnisme");
        DesactiveImp();
        transform.Find("Q2Imp")?.gameObject.SetActive(true);
    }

    public void ActiveQ3Imp(){
        Debug.Log("Activation Réponse Q3 Impressionnisme");
        DesactiveImp();
        transform.Find("Q3Imp")?.gameObject.SetActive(true);
    }

    public void ActiveQ4Imp(){
        Debug.Log("Activation Réponse Q4 Impressionnisme");
        DesactiveImp();
        transform.Find("Q4Imp")?.gameObject.SetActive(true);
    }

    public void ActiveQ5Imp(){
        Debug.Log("Activation Réponse Q5 Impressionnisme");
        DesactiveImp();
        transform.Find("Q5Imp")?.gameObject.SetActive(true);
    }

}
