using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class cubeAgent : Agent
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Rigidbody rb;
    
    //same coordinates as cube
    public Vector3 startPosition;
    private bool despawnObjects = false;

    private bool jump = true;

    public override void OnEpisodeBegin()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        //transform.localPosition = startPosition;
        transform.localRotation = Quaternion.identity;
    }

    public int RandomItemType;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(RandomItemType);
    }

    public void giveRewardExternally(float rewardAmount)
    {
        Debug.Log($"reward of {rewardAmount} given");
        SetReward(rewardAmount);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;

        // Interpret the actions for left and right movement
        float horizontalInput = actions.ContinuousActions[0]; // Assuming the first continuous action controls left/right movement

        // Apply left/right movement
        controlSignal.x = horizontalInput * moveSpeed;

        // Check if the agent falls below a certain threshold
        if (transform.localPosition.y < -1.0f)
        {
            SetReward(-1.0f); // Penalize for falling
            transform.localPosition = startPosition;
            EndEpisode(); // Reset the episode
        }

        // Apply jump action
        if (actions.DiscreteActions[0] == 1 && jump)
        {
            jump = false;
            Debug.Log("Jumping!");
            SetReward(-0.01f);
            rb.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);
        }

        /*else
        {
            SetReward(0.05f); // Small reward for not jumping
        }*/

        // Check if the agent is too much to the left or right
        /*float xPos = transform.localPosition.x;
        if (xPos < -2.24f || xPos > 2.24f) // Define your desired range here
        {
            Debug.Log("Too much on the sides");
            SetReward(-0.01f); // Penalize for being out of desired range
        }*/

        // Apply movement
        transform.Translate(controlSignal * Time.deltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        var action = actionsOut.DiscreteActions;
        action.Clear();

        // Jump
        if (Input.GetAxis("Vertical") > 0)
        {
            action[0] = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("reward"))
        {
            SetReward(0.7f);
            Debug.Log("Hit reward sphere!!!");
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("obstacle"))
        {
            SetReward(-1.0f);
            Debug.Log("Obstacle hit!!!");
            foreach (var item in GameObject.FindGameObjectsWithTag("obstacle").Concat(GameObject.FindGameObjectsWithTag("reward")))
            {
                Destroy(item);
            }
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("floor"))
        {
            jump = true;
        }
    }
}
