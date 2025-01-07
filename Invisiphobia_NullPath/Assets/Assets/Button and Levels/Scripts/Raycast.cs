using UnityEngine;
using UnityEngine.UI;


public class Raycast : MonoBehaviour

{
    [SerializeField] private Text _text;
    [SerializeField] private float rayDistanse;
    [SerializeField] private LayerMask layerMask;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        RaycastInfo();
    }

    private void RaycastInfo()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * rayDistanse, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistanse, layerMask))
        {
                _text.enabled = true;

                if (Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Button>() != null)
                {
                    hit.collider.GetComponent<Button>().Interaction();
                }

                if (Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Button2>() != null)
                {
                    hit.collider.GetComponent<Button2>().Interaction();
                }

                if (Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Switch>() != null)
                {
                    hit.collider.GetComponent<Switch>().Interaction();
                }

                if(Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Switch2>() != null)
                {
                    hit.collider.GetComponent<Switch2>().Interaction();
                }

                if(Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Lever>() != null)
                {
                    hit.collider.GetComponent<Lever>().Interaction();
                }

                if(Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Lever2>() != null)
                {
                    hit.collider.GetComponent<Lever2>().Interaction();
                }

                if(Input.GetKeyDown(KeyCode.E) && hit.collider.GetComponent<Fauset>() != null)
                {
                    hit.collider.GetComponent<Fauset>().Interaction();
                }
        }
        else 
        { 
            _text.enabled = false; 
        }

    }
    
}
