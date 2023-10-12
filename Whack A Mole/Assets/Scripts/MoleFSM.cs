using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// ���Ͽ� ���, ���� ��� , ���� > ���� �̵� , ���� > ���� �̵�

public enum MoleState { UnderGround = 0, OnGround, MoveUp, MoveDown }
public class MoleFSM : MonoBehaviour
{
    [SerializeField]
    private float waitTimeOnGround;      // ���鿡 �ö�ͼ� ����������� ��ٸ��� �ð�
    [SerializeField]
    private float limitMinY;             // ������ �� �ִ� �ּ� y ��ġ
    [SerializeField]
    private float limitMaxY;             // �ö�� �� �ִ� �ִ� y ��ġ 

    private Movement3D movement3D;       // ��/�Ʒ� �̵��� ���� Movement3D

    // �δ����� ���� ���� (set�� MoleFSM Ŭ���� ���ο�����)
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