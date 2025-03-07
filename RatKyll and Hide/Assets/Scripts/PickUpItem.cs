using UnityEngine;
using UnityEngine.UIElements;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody rb;
    private Transform PickUpPointOne;
    private Transform PickUpPointTwo;
    private Transform playerOneTransform;
    private Transform playerTwoTransform;
    [SerializeField] float pickUpDistanceOne;
    [SerializeField] float pickUpDistanceTwo;
    [SerializeField] float throwForce;
    [SerializeField] bool readytoThrow;
    [SerializeField] bool itemisPicked;
    [SerializeField] bool attachedToPlayerOne;
    [SerializeField] bool attachedToPlayerTwo;
    private bool isCharging = false;
    private GameObject currHolder;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOneTransform = GameObject.Find("Player1").transform;
        playerTwoTransform = GameObject.Find("Player2").transform;
        PickUpPointOne = playerOneTransform.Find("PickUpPoint").transform;
        PickUpPointTwo = playerTwoTransform.Find("PickUpPoint").transform;
    }

    void Update()
    {
        pickUpDistanceOne = Vector3.Distance(playerOneTransform.position, transform.position);
        pickUpDistanceTwo = Vector3.Distance(playerTwoTransform.position, transform.position);
        if (!itemisPicked)
        {
            HandlePickup();
        }
        else
        {
            HandleThrow();
        }
    }
    
    void HandlePickup()
    {
        if (pickUpDistanceOne <= 2 && Input.GetKeyDown(KeyCode.E) && PickUpPointOne.childCount < 1)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = true;
            this.transform.position = PickUpPointOne.position;
            this.transform.parent = PickUpPointOne;
            itemisPicked = true;
            throwForce = 0;
            attachedToPlayerOne = true;
        }
        
        if (pickUpDistanceTwo <= 2 && Input.GetKeyDown(KeyCode.E) && PickUpPointTwo.childCount < 1)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = true;
            this.transform.position = PickUpPointTwo.position;
            this.transform.parent = PickUpPointTwo;
            itemisPicked = true;
            throwForce = 0;
            attachedToPlayerTwo = true;
        }
    }
    
    void HandleThrow()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCharging = true;
            readytoThrow = true;
        }
        
        if (isCharging && Input.GetKey(KeyCode.E))
        {
            throwForce += 300 * Time.deltaTime;
        }
        
        if (readytoThrow && Input.GetKeyUp(KeyCode.E))
        {
            if (throwForce > 10)
            {
                if (attachedToPlayerOne)
                {
                    rb.AddForce(playerOneTransform.forward * throwForce);
                    ReleaseObject();
                    attachedToPlayerOne = false;
                }
                else if (attachedToPlayerTwo)
                {
                    rb.AddForce(playerTwoTransform.forward * throwForce);
                    ReleaseObject();
                    attachedToPlayerTwo = false;
                }
            }
            else
            {
                ReleaseObject();
                if (attachedToPlayerOne) attachedToPlayerOne = false;
                if (attachedToPlayerTwo) attachedToPlayerTwo = false;
            }
            
            isCharging = false;
            readytoThrow = false;
        }
    }
    
    void ReleaseObject()
    {
        this.transform.parent = null;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        itemisPicked = false;
        throwForce = 0;
    }
}
