using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PNJTextField : MonoBehaviour
{
    private Transform m_playerAnchor;

    [SerializeField]
    private float rotationSpeed = 5f;


    private void TurnToPlayer()
    {
        Vector3 direction = transform.position - m_playerAnchor.position;
        direction.y = 0; // Ignore l'axe Y
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    void Start()
    {
        m_playerAnchor = Camera.main.transform;

        if (m_playerAnchor == null)
        {
            Debug.Log("[PNJTextField] : Ne trouve pas la main Camera.");
        }
    }


    void Update()
    {
        TurnToPlayer();   
    }
}
