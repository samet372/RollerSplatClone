using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{   GameManager gm;

    Rigidbody rb;

    public Image _levelBar;

    Vector2 _firstPos;

    Vector2 _secondPos;

    Vector2 _currentPos;

    [SerializeField]
    float _moveSpeed;

    public float _currentGroundNumber;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindObjectOfType<GameManager>();
        _levelBar.fillAmount = 0;
    }


    private void Update()
    {
        Swipe();

        _levelBar.fillAmount = _currentGroundNumber / gm._groundNumbers;

        if (_levelBar.fillAmount == 1)
        {
           gm.LevelUpdate();
        }
    }

    void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _secondPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            _currentPos = new Vector2(
                 _secondPos.x - _firstPos.x,
                 _secondPos.y - _firstPos.y
                );
        }

        _currentPos.Normalize();

        if (_currentPos.y < 0 && _currentPos.x > -0.5f && _currentPos.x < 0.5f)
        {
            rb.velocity = Vector3.back * _moveSpeed;
        }
        else if (_currentPos.y > 0 && _currentPos.x > -0.5f && _currentPos.x < 0.5f)
        {
            rb.velocity = Vector3.forward * _moveSpeed;
        }
        else if (_currentPos.x < 0 && _currentPos.y > -0.5f && _currentPos.y < 0.5f)
        {
            rb.velocity = Vector3.left * _moveSpeed;
        }
        else if (_currentPos.x > 0 && _currentPos.y > -0.5f && _currentPos.y < 0.5f)
        {
            rb.velocity = Vector3.right * _moveSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MeshRenderer>().material.color != Color.red)
        {
            if (collision.gameObject.tag == "Ground")
            {
                collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                Constraints();
                _currentGroundNumber++;
            }
        }   
    }

    void Constraints()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;       
    }
}
