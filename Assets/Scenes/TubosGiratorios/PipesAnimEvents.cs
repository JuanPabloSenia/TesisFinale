using UnityEngine;

public class PipesAnimEvents : MonoBehaviour
{

    public GameObject smth;

    public void SetCanRotateTrue()
    {
        transform.parent.GetComponent<PipeBehaviour>().SetCanRotateTrue();
    }
}