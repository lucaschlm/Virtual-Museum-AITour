using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*######################################################*/
/* DontDestroy :                                        */
/* - S'occupe de la destruction des objets doublons     */
/*######################################################*/
public class DontDestroy : MonoBehaviour {
    void Start(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag(this.tag);
        if (objs.Length > 1){
            Destroy(this.gameObject); // S'il y a déjà 3 piles, on supprimes les autres
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
