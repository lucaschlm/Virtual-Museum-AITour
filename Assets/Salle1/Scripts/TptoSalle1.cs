using UnityEngine;
using System; 
using System.Collections;
using UnityEngine.SceneManagement;

public class TptoSalle1 : MonoBehaviour{


    private void Awake(){
        
    }


    IEnumerator LoadSceneAsync(string name){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Salle1");
        asyncLoad.allowSceneActivation = false;
        // Empeche l'activation de la scene 

        while(asyncLoad.progress < 0.9f){ //attend que 90% soit chargé
            yield return null; // Attend la frame d'après 
        }

        yield return new WaitForSeconds(0.5f); // attend une demi seconde

        asyncLoad.allowSceneActivation = true; // Active la scène

        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            

            // Chargement de l'autre scène doucement
            StartCoroutine(LoadSceneAsync("Salle1"));

            // Placement du joueur au bon endroit
            GameObject[] joueur = GameObject.FindGameObjectsWithTag("Player");
            joueur[0].transform.position = new Vector3(4, 0, -3.5f);
        }
    }


    private void Update(){

    }
}
