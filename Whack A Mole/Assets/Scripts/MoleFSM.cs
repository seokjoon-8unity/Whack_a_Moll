using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// 지하에 대기, 지상에 대기 , 지하 > 지상 이동 , 지상 > 지하 이동

public enum MoleState { UnderGround = 0, OnGround, MoveUp, MoveDown }
public class MoleFSM : MonoBehaviour
{
    [SerializeField]
    private float waitTimeOnGround;      // 지면에 올라와서 내려가기까지 기다리는 시간
    [SerializeField]
    private float limitMinY;             // 내려갈 수 있는 최소 y 위치
    [SerializeField]
    private float limitMaxY;             // 올라올 수 있는 최대 y 위치 

    private Movement3D movement3D;       // 위/아래 이동을 위한 Movement3D

    // 두더지의 현재 상태 (set은 MoleFSM 클래스 내부에서만)
    public MoleState Molestate { private set; get; }

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        ChangeState(MoleState.UnderGround);
    }

    public void ChangeState(MoleState newState)
    {
        StopCoroutine(Molestate.ToString());
        Molestate = newState;
        StartCoroutine(Molestate.ToString());
    }

    private IEnumerator UnderGround()
    {
        movement3D.MoveTo(Vector3.zero);
        transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

        yield return null;
    }

    private IEnumerator OnGround()
    {
        movement3D.MoveTo(Vector3.zero);
        transform.position = new Vector3(transform.position.x, limitMaxY, transform.position.z);

        yield return new WaitForSeconds(waitTimeOnGround);

        ChangeState(MoleState.MoveDown);
    }

    private IEnumerator MoveUp()
    {
        movement3D.MoveTo(Vector3.up);

        while (true)
        {
            if (transform.position.y >= limitMaxY)
            {
                ChangeState(MoleState.OnGround);
            }

            yield return null;
        }
    }

    private IEnumerator MoveDown()
    {
        movement3D.MoveTo(Vector3.down);

        while ( true )
        {
            if ( transform.position.y <= limitMinY )
            {
                ChangeState(MoleState.UnderGround);
            }    

            yield return null;
        }
    }
}