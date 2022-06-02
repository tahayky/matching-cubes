using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PlayerPhysics : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private PlayerContacts player_contacts;
    private BoxCollider box_collider;
    #endregion
    private void Awake()
    {
        box_collider = GetComponent<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            player_contacts.CubeContact(collision.transform);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            player_contacts.ObstacleContact(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Turbo"))
        {
            player_contacts.TurboContact();
        }
        else if (collision.gameObject.CompareTag("Ramp"))
        {
            player_contacts.RampContact();
        }
        else if (collision.gameObject.CompareTag("Gate"))
        {
            Gate gate_type = collision.gameObject.GetComponent<Gate>();
            player_contacts.GateContact(gate_type);
        }
        else if (collision.gameObject.CompareTag("Overpass"))
        {
            OverPass overpass = collision.gameObject.GetComponent<OverPass>();
            player_contacts.OverpassContact(overpass);
        }
        else if (collision.gameObject.CompareTag("Fin"))
        {
            player_contacts.FinishContact();
        }
        else if (collision.gameObject.CompareTag("Hole"))
        {
            player_contacts.HoleContact();
        }
    }
    #region Commands
    public void ReSizeCollider(float _height)
    {
        box_collider.size = new Vector3(box_collider.size.x, _height, box_collider.size.z);
        box_collider.center = Vector3.up * _height / 2;
    }
    #endregion
}
