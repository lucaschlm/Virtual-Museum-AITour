using UnityEngine;
using System; 
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleport : MonoBehaviour{

    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private string scene;
    
    private GameManager gameManager;  

    void Start(){
        Debug.Log("Début du start");
        StartCoroutine(FadeFromBlack()); // Retire le noir au début
        GameObject GM = GameObject.Find("GameManager");
        gameManager = GM.GetComponent<GameManager>();
        if(gameManager != null){
            if(gameObject.CompareTag("TptoNext")){
                if(gameManager.isValidee()){ // ici 
                    gameObject.SetActive(true);
                    // Debug.Log("Objet Activé");
                } else {
                    // Debug.Log("Objet Désactivé");
                    gameObject.SetActive(false);
                }
            }
        } else {
            Debug.Log("GameManager non trouvée");
        }
    }


    IEnumerator LoadSceneAsync(string name){

        yield return StartCoroutine(FadeToBlack());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;
        // Empeche l'activation de la scene 

        while(asyncLoad.progress < 0.9f){ //attend que 90% soit chargé
            yield return null; // Attend la frame d'après 
        }

        yield return new WaitForSeconds(0.5f); // attend une demi seconde

        asyncLoad.allowSceneActivation = true; // Active la scène

        gameManager.Move(name);
    }

    public void RetireNoir(){
        StartCoroutine(FadeFromBlack());
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            StartCoroutine(LoadSceneAsync(scene));
        }
    }

    IEnumerator FadeToBlack(){        
        for(float i = 0; i <= 1; i += Time.deltaTime){
            fadeImage.color = new Color(0,0,0,i);
            yield return null;
        }
    }

    IEnumerator FadeFromBlack(){
        for(float i = 1; i >= 0; i -= Time.deltaTime){
            fadeImage.color = new Color(0,0,0,i);
            yield return null;
        }
    }

    private void Update(){

    }
}
