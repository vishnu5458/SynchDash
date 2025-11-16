using UnityEngine;

public class RemotePlayer : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.remoteHandler += RecieveDataFromServer; //subscribe method to read data from server
    }

    private void OnDisable()
    {
        PlayerController.remoteHandler -= RecieveDataFromServer; //Here unsubcribe method
    }

    //Here read data from server
    void RecieveDataFromServer(float _xValue, float _yValue, float _zValue)
    {
        Vector3 receivedPosition = new Vector3(_xValue, _yValue, _zValue);

        transform.localPosition = receivedPosition; //Here set position
        Debug.Log($"Recieved:{receivedPosition}");  //Print recieved Dequantize value
    }
}
