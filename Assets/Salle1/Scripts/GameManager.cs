using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Liste doublement chaînée avec le nom et un bool si le joueur à validé le quizz ? 
    private DoublyLinkedList Liste;
    private Node Current;
    private GameObject TptoNext;

    void Start(){
        Liste = new DoublyLinkedList();
        Liste.Add("Start", true);
        Current = Liste.head;
        TptoNext = GameObject.FindGameObjectWithTag("TptoNext");
        // Debug.Log("Noeud actuel : " + Current.Salle);
    } 

    // Change de salle
    public void Move(string scene){
        Debug.Log("Bon appel à Move");
        if(Current.Prev != null && scene == Current.Prev.Salle){ // Retour en arrière
            Current = Current.Prev;
        } else if ( Current.Next != null && scene == Current.Next.Salle){ // Avance en avant
            Current = Current.Next;
        } else { // Nouvelle salle
            Liste.Add(scene,false);
            Current = Current.Next;
        }
    }

    // renvoie vrai si la salle est validée
    public bool isValidee(){
        return Current.Validee;
    }

    // Valide la scène et affiche le Tp
    public void Valide(bool v){
        Current.Validee = v;
        if(TptoNext != null){
            TptoNext.SetActive(v);
        }
    }


    void Update(){
    }
}


class Node
{
    public string Salle { get; set; }
    public bool Validee { get; set; }
    public Node Prev { get; set; }
    public Node Next { get; set; }

    public Node(string salle, bool validee)
    {
        Salle = salle;
        Validee = validee;
        Prev = null;
        Next = null;
    }


}

class DoublyLinkedList
{
    public Node head {get; set;}
    public Node tail {get; set;}

    public void Add(string salle, bool validee)
    {
        Node newNode = new Node(salle, validee);
        if (head == null)
        {
            head = newNode; 
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            newNode.Prev = tail;
            tail = newNode;
        }
    }

    public void Remove(string salle)
    {
        Node current = head;
        while (current != null)
        {
            if (current.Salle == salle)
            {
                if (current.Prev != null)
                    current.Prev.Next = current.Next;
                else
                    head = current.Next;

                if (current.Next != null)
                    current.Next.Prev = current.Prev;
                else
                    tail = current.Prev;
                
                return;
            }
            current = current.Next;
        }
    }

    public void Display()
    {
        Node current = head;
        while (current != null)
        {
            Debug.Log($"Salle: {current.Salle}, Validée: {current.Validee}");
            current = current.Next;
        }
    }
}
