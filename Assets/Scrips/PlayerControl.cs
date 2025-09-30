using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
     public GameObject PlayerBulletGo;
    public GameObject bulletPosition1;
    public GameObject bulletPosition2;
    public GameObject ExplosionGO;
    public TextMeshProUGUI LiveUiText;
    const int Maxlives = 3;
    int lives;
    
    public float speed;
     void Start()
    {
        
    }
    public void Init()
    {
        lives = Maxlives;
        LiveUiText.text = lives.ToString();
        gameObject.SetActive(true);
    }
     void Update()
    {
        // fire bullet when the spacebar is press
        if (Input.GetKeyDown("space"))
        {
            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGo);
            bullet01.transform.position = bulletPosition1.transform.position;


            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGo);
            bullet02.transform.position = bulletPosition2.transform.position;

        }
        float x = Input.GetAxisRaw("Horizontal"); // gia tri -1(left),0(no input),1(right)
        float y = Input.GetAxisRaw("Vertical"); // gia tri -1(xuong),0(no input),-1(len)
        Vector2 direction = new Vector2(x, y).normalized;
        // goi function de tinh toan va thiet lap vi tri player
        Move (direction);


    }
    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0)); // điểm dưới cùng bên trái (góc) của màn hình
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1)); // điểm trên cùng bên phải (góc) của màn hình
        max.x = max.x - 0.225f; // trừ đi một nửa chiều rộng của sprite người chơi
        min.x = min.x - 0.225f;// thêm một nửa chiều cao của nhân vật người chơi

        //Nhận vị trí hiện tại người chơi 
        Vector2 pos = transform.position;

        // Tính toán vị trí mới 
        pos += direction * speed * Time.deltaTime;

        //Đảm bảo vị trí mới không nằm ngoài màn hình
        pos.x = Mathf.Clamp(pos.x,min.x, max.x);
        pos.y = Mathf.Clamp(pos.y,min.y, max.y);

        //Cập nhật vị trí mới của player
        transform.position = pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "EnemyShipTag") || (collision.tag == "EnemyBulletTag"))
        {
            PlayExplosion();
            lives--;
            LiveUiText.text = lives.ToString();
            if (lives == 0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                //Destroy(gameObject); 
                gameObject.SetActive(false);

            }
          
        }
    }
    void PlayExplosion()
    {
        GameObject explo = (GameObject)Instantiate(ExplosionGO);
        explo.transform.position = transform.position;
    }
}
