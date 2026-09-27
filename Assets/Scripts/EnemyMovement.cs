using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject targetObject;
    private Vector3 targetPos;
    private NavMeshAgent aiBrain;
    private bool canMove = true;
    [SerializeField] private float timeBetweenEachPositionSample = 0.2f;
    [SerializeField] private float rewindDuration = 10f;
    private float timerToRecordPosition = 0;
    private float timerToReducePrevPosQueue = 0;
    private Stack<Vector3> previousPosStack = new Stack<Vector3>();
    private Queue<Vector3> previousPosWindow = new Queue<Vector3>();
    private Vector3 prevPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevPos = transform.position;
        targetObject = GameObject.FindGameObjectWithTag("Player");
        aiBrain = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        timerToRecordPosition += Time.deltaTime;
        if (timerToRecordPosition >= timeBetweenEachPositionSample)
        {
            SavePreviousPos();
            timerToRecordPosition = 0;
        }
        timerToReducePrevPosQueue += Time.deltaTime;
        if (timerToReducePrevPosQueue >= rewindDuration)
        {
            HandleRewindWindow();
            timerToReducePrevPosQueue = 0;
        }
    }
    public void MoveTowardsTarget()
    {
        if (!canMove) return;

        targetPos = targetObject.transform.position;
        aiBrain.SetDestination(targetPos);
        SavePreviousPos();
        HandleRewindWindow();
    }
    private void SavePreviousPos()
    {
        if (!(Vector3.Distance(transform.position, prevPos) < 0.01f))
        {
            previousPosWindow.Enqueue(transform.position);
            prevPos = transform.position;
        }
        
    }
    private void HandleRewindWindow()
    {
        int queuelimit = (int)(rewindDuration * (1 / timeBetweenEachPositionSample));
        while(previousPosWindow.Count>0 && previousPosWindow.Count>15)
            previousPosWindow.Dequeue();
        
    }
    public void SetCanMove(bool cm)
    {
        canMove = cm;
    }
    public void MoveAwayFromTarget()
    {
        StartCoroutine(RewindingSeq());

    }
    private IEnumerator RewindingSeq()
    {
        LoadPosToStack();
        while (previousPosStack.Count > 0)
        {
            targetPos = previousPosStack.Pop();
            aiBrain.enabled = false;
            float lerpTime = 0;
            Vector3 currPos = transform.position;
            while(! (Vector3.Distance(transform.position,targetPos)<0.1f))
            {
                lerpTime += Time.deltaTime;
                float lerpPercent  = lerpTime / timeBetweenEachPositionSample;
                transform.position = Vector3.Lerp(currPos, targetPos, lerpPercent);
                yield return null;
            }
        }
            yield return null;
        aiBrain.enabled = true;
    }
    private void LoadPosToStack()
    {
        while (previousPosWindow.Count > 0)
        {
            Vector3 temp = previousPosWindow.Dequeue();
            previousPosStack.Push(temp);
        }
    }
}
