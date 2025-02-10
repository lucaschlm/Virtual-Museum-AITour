using System;
using UnityEngine;

class GetInfo : MonoBehaviour {

    [SerializeField]
    private String message;

    void Start(){
        message = "Les Noces de Cana\n Cette immense peinture de Véronèse illustre un festin biblique où le Christ accomplit son premier miracle : transformer leau en vin. Je peux vous guider vers cette œuvre, qui se trouve dans la salle dédiée à la Renaissance.";
        getOeuvre();
    }

    private string getOeuvre(){
        string oeuvre = message.Split('\n')[0]; 
        Debug.Log(oeuvre);
        return oeuvre;
    }


    void Update(){

    }

}