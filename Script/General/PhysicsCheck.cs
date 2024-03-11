using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    //������
    public float checkRadius;
    public LayerMask layerGround;
    public Vector2 pointOffset;
    public bool isGround;

    //ײǽ���
    public bool touchWall;
    public Vector2 wallOffset;

    //private Collider2D coll;

    private void Awake()
    {
        //�Զ�����ƫ����
        //coll = GetComponent<CapsuleCollider2D>();
        //rightOffset = new Vector2(coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
    }

    private void Update()
    {
        check();
    }

    private void check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+pointOffset, checkRadius, layerGround);
        touchWall = Physics2D.OverlapCircle((Vector2)transform.position + wallOffset, checkRadius, layerGround);      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + pointOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallOffset, checkRadius);
    }
}
