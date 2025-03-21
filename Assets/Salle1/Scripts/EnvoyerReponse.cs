using System;
using UnityEngine;
using UnityEngine.SceneManagement;

class EnvoyerReponse : MonoBehaviour {

    void Start(){

    }
    
    void Update(){

    }

    /*----------------------------*/
    /*        RENAISSANCE         */
    /*----------------------------*/

    // QUESTION 1 : Quelle oeuvre représente un mariage
    public void AQ1Ren(){ // Les Noces de Cana
        string prompt = "Ce sont les Noces de Cana (The Wedding at Cana) qui représente une scène de mariage";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ1Ren(){// La Cène
        string prompt = "La Cène";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ1Ren(){// Le Jardin des délices
        string prompt = "Le Jardin des délices";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // QUESTION 2 : Quel artiste a sculpté Moïse
    public void AQ2Ren(){ // Léonard de vinci
        string prompt = "Léonard de vinci  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ2Ren(){// Francesco da Sangallo
        string prompt = "Francesco";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ2Ren(){// Michel-Ange
        string prompt = "C'est Michel-Ange (Michelangelo) qui a sculpté Moïse";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // QUESTION 3 : Quelle sculpture représente une femme symbolise la mer
    public void AQ3Ren(){ // Le buste d'Annibal
        string prompt = "Le buste d'Annibal";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ3Ren(){// L'enfant à l'oie
        string prompt = "L'enfant à l'oie";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ3Ren(){// Amphitrite
        string prompt = "C'est l'Amphitrite qui est une sculpture qui symbolise la mer";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // QUESTION 4 : Quel sculpture représente celui qui a combatue Goliath
    public void AQ4Ren(){ // David
        string prompt = "David";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ4Ren(){// Annibal
        string prompt = "Annibal";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ4Ren(){// Moïse
        string prompt = "Moïse";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // QUESTION 5 : Quel peinture te suis du regard
    public void AQ5Ren(){ // La Madone Sixtie
        string prompt = "Madone Sixtine";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ5Ren(){// La Joconde
        string prompt = "C'est la Joconde, connu sous le nom de Mona Lisa qui te suis du regard";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ5Ren(){// La Naissance de vénus
        string prompt = "La Naissance de Vénus";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    /*----------------------------*/
    /*      IMPRESSIONNISME       */
    /*----------------------------*/

    // Question 1 : Qui a peint Impression, soleil Levant
    public void AQ1Imp(){ //Toulouse Lautrec
        string prompt = "Toulouse Lautrec";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ1Imp(){ // Claude Monet
        string prompt = "C'est Claude Monet qui a peint Impression, soleil Levant";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ1Imp(){ // P-A Renoir
        string prompt = "Renoir";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // Question 2 : Quelle est l'atmosphère de l'Absynthe
    public void AQ2Imp(){ //Triste 
        string prompt = "Dans le tableau l'Absynthe, l'atmosphère est vraiment triste";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ2Imp(){ // Joyeuse 
        string prompt = "Joyeuse";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ2Imp(){ // neutre 
        string prompt = "L'atmosphère est normal  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // Question 3 : De quel pays viennent la plus par des tableau
    public void AQ3Imp(){ // Autriche 
        string prompt = "Autriche  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ3Imp(){ // France
        string prompt = "La pluspart des tableaux viennent de France";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ3Imp(){ // Allemagne 
        string prompt = "Allemagne  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // Question 4 : Quel instrument se trouve dans la classe de danse
    public void AQ4Imp(){ // Violon
        string prompt = "C'est un violon (ou piano) qui se trouve dans la classe de danse de Degas ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ4Imp(){ // Harpe
        string prompt = "une harpe";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ4Imp(){ // guitare
        string prompt = "Guitare";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    // Question 5 : Qui a peint Boulevard MontMartre 
    public void AQ5Imp(){ // Degas
        string prompt = "Degas  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void BQ5Imp(){ // Manet
        string prompt = "Manet  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    public void CQ5Imp(){ // Pissaro
        string prompt = "C'est Camille Pissarro qui a peint Boulevard MontMartre , effet de nuit  ";
        EventManager.Instance.TriggerOnAddedToPrompt(prompt);
        EventManager.Instance.TriggerRequestSended(); 
    }
}