using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float score;
    private PlayerController playerControllerScript;

    public Transform startingPoint;
    public float lerpSpeed;

    public GameObject spawnManager;
    private ParticleSystem.MainModule pMain;
    private ParticleSystem.VelocityOverLifetimeModule vOL;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        score = 0.0f;

        pMain = playerControllerScript.dirtParticle.main;
        vOL = playerControllerScript.dirtParticle.velocityOverLifetime;
        playerControllerScript.playerAnim.SetFloat("Speed_f", 0.30f);
        playerControllerScript.playerAnim.SetBool("Static_b", true);
        pMain.startSpeed = 0.5f;
        pMain.simulationSpeed = 0.2f;
        vOL.speedModifier = 0.5f;

        playerControllerScript.isGameOver = true;
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.isGameOver)
        {
            if (playerControllerScript.dashSpeed)
            {
                score += 2 * Time.deltaTime;
            }
            else
            {
                score += 1 * Time.deltaTime;
            }
            Debug.Log("Score: " + Mathf.Floor(score));
        }
    }

    IEnumerator PlayIntro()
    {
        Vector3 playerCurrentPos = playerControllerScript.transform.position; // Get the players position
        Vector3 endPos = startingPoint.position;

        float journeyLength = Vector3.Distance(playerCurrentPos, endPos); // The distance between the players current pos and the starting point of the game
        float distanceCovered = Time.time * lerpSpeed; // Distance = time * speed, Distance = speed * time, d = st

        float fractionOfJourney = distanceCovered / journeyLength;
        // The distance travelled divided by the journey, giving us a % of how much we have travelled from 0 to 1.
        // It will allow us to move our player in very small increments.
        // Even though they are both distances, the fractionOfJourney we plug into the player's postion
        // equates to: Time = Distance/Speed. In this case Speed/Distance. journeyL being the distance, and distanceCov being the speed.
        // Which gives us fractionOfJourney as our time for the lerp function.


        //playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 0.5f);
        //while (fractionOfJourney < 1)

        while (distanceCovered < journeyLength)
        {
            distanceCovered = Time.time * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerControllerScript.transform.position = Vector3.Lerp(playerCurrentPos, endPos, fractionOfJourney);

            //Debug.Log("Journey: " + journeyLength + "; Dist: " + distanceCovered + "; Fract; " + fractionOfJourney + "; Time: " + Time.time);

            yield return null;
        }

        //playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f);

        pMain = playerControllerScript.dirtParticle.main;
        vOL = playerControllerScript.dirtParticle.velocityOverLifetime;
        playerControllerScript.playerAnim.SetFloat("Speed_f", 1f);
        playerControllerScript.playerAnim.SetBool("Static_b", false);
        pMain.startSpeed = 5f;
        pMain.simulationSpeed = 0.65f;
        vOL.speedModifier = 2f;

        playerControllerScript.isGameOver = false;

        spawnManager.SetActive(true);
    }
}
