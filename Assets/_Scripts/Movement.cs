using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    Rigidbody       rigid;
    bool collideToWall;

    float xAxis;
    [SerializeField] bool doJump;
    [SerializeField] bool isJump;

    void Start()
    {
        rigid       = GetComponent<Rigidbody>();
    }

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            doJump = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    public void Move()
    {
        if (transform.localScale.x < 0 && xAxis > 0)
        {
            Vector3 reverse = transform.localScale;
            reverse.x = -transform.localScale.x;
            transform.localScale = reverse;
        }
        if (transform.localScale.x > 0 && xAxis < 0)
        {
            Vector3 reverse = transform.localScale;
            reverse.x = -transform.localScale.x;
            transform.localScale = reverse;
        }

        Vector3 velocity = new Vector3(xAxis, 0, 0);
        velocity = velocity * moveSpeed;

        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
    }

    public void Jump()
    {
        if (!doJump) return;

        doJump = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // ===============================================================================================
    // �÷����� �����鿡 ��Ҵ��� Ȯ���ϴ� �Լ�
    // �÷��̾�� �浹������ ���ϴ� ����(collision.contacts[0].point - transform.position)��
    // (0, -1, 0) ���͸� �����Ͽ� �÷����� �÷��̾��� �浹 ���¸� ����.
    // 0.75�� ������ ��. ���� �� ���� ����
    // �ùٸ� �浹 �� SmoothJump �ڷ�ƾ�� ���߰�, �ٽ� ������ �� �ִ� ���·� ��ȯ
    // ������ ����Ͽ� ����� ���� : collision.contacts[n].normal.y�� ��� �� �÷����� �Ʒ�����
    // �÷��̾ �����Ͽ� �������� ��Ƶ� �ùٸ� �浹�� �ν��ϱ� ������ ���� �÷��̾��� �߷κ���
    // ���� ������ �浹�� ó���ϱ� ����
    // ===============================================================================================
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (Vector3.Dot(collision.contacts[0].point - transform.position, Vector3.down) > 0.75f)
            {
                isJump = false;
                StopCoroutine(SmoothJump());
            }
        }
    }

    // ===============================================================================================
    // �÷����� �� �κп� ��Ҵ��� �˻��ϴ� �Լ�
    // �ѹ��̶� ���� �浹�ߴٸ� collideToWall ������ True�� ����.
    // �ݺ����� ����Ͽ� �˻��ϴ� ������ ���ÿ� ���� ��ü�� �浹 ���°� �� �� �ֱ� ����.
    // ===============================================================================================
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Platform" && !collideToWall)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector3.Dot(collision.contacts[i].point - transform.position, Vector3.down) <= 0.75f)
                {
                    collideToWall = true;
                    break;
                }
            }
        }
    }

    // ===============================================================================================
    // �÷����� �ٴڿ��� ��� ���� ȣ��Ǵ� �Լ�
    // ������ ����ٸ� ȣ����� ������, �÷������� �������� �ʰ� �̲��� ������ ���¶�� �ڷ�ƾ �����Ͽ�
    // ���� �ð� �ڿ� ���� �Ұ� ���·� ��ȯ
    // ===============================================================================================
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform" && !collideToWall)
        {
            StartCoroutine(SmoothJump());
        }
    }

    // ===============================================================================================
    // �ε巯�� ���� �����ϱ� ���� �Լ�
    // ���� �Լ��� �ִ� �� �и�
    // ===============================================================================================
    private IEnumerator SmoothJump()
    {
        yield return new WaitForSeconds(0.2f);
        isJump = true;
    }
}
