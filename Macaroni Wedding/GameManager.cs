using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public int level = 1;
    public float mapHeight = 18;
    public float mapWidth = 16;
    int killed = 0;
    bool gameOver = false;

    [SerializeField]
    private AudioClip no_male;
    [SerializeField]
    private AudioClip no_female;

    int toNextLevel = 10;
    int currlevel;
    float procent;
    string kto = "";

    GameObject male;
    GameObject female;
    EnemySpawner EnemySpawnerFemale;
    EnemySpawner EnemySpawnerMale;

    AudioSource audio;
    public TextAsset textAsset;
    string text;
    string[] sentences, words;

    public Text obj;

    int prevTextLength;

    public Animator animator;

    void Start()
    {
        Time.timeScale = 1f;
        male = GameObject.FindGameObjectWithTag("male");
        female = GameObject.FindGameObjectWithTag("female");
        EnemySpawnerFemale = GameObject.Find("Enemy Spawner female").GetComponent<EnemySpawner>();
        EnemySpawnerMale = GameObject.Find("Enemy Spawner male").GetComponent<EnemySpawner>();

        audio = GameObject.Find("audio").GetComponent<AudioSource>();

        sentences = textAsset.text.Split('.');
        obj.text = "";
        words = sentences[0].Split(' ');
        currlevel = toNextLevel;
        procent = 1 / toNextLevel;
        prevTextLength = 0;
    }

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!gameOver)
        {
            if (male.transform.position.x > 0)
            {
                Vector3 pos = male.transform.position;
                pos.x = -0.5f;
                male.transform.position = pos;
            }
            if (female.transform.position.x < 0)
            {
                Vector3 pos = female.transform.position;
                pos.x = 0.5f;
                female.transform.position = pos;
            }


            /*if (Mathf.Abs(male.transform.position.x) > mapWidth || Mathf.Abs(male.transform.position.z) > mapHeight)
            {
                kto = "male";
                Time.timeScale = 0.5f;
                Invoke("GameOver", 2.5f* 0.5f);
                
                audio.PlayOneShot(no_female);
                
                gameOver = true;
            }
            if (Mathf.Abs(female.transform.position.x) > mapWidth || Mathf.Abs(female.transform.position.z) > mapHeight)
            {
                kto = "female";
                Time.timeScale = 0.5f;
                Invoke("GameOver", 2.5f* 0.5f);
                
                audio.PlayOneShot(no_male);
                
                gameOver = true;
            }*/
        }
	}
    public void KonczGre(string tag)
    {
        if(!gameOver)
        {
            kto = tag;
            Time.timeScale = 0.5f;
            Invoke("GameOver", 2.5f * 0.5f);
            AudioClip sound = no_male;
            if (tag == "female")
                sound = no_female;
            audio.PlayOneShot(sound);

            gameOver = true;
        }
        
    }
    void GameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(kto+"_dead", LoadSceneMode.Single);
        Debug.Log("Die by " + kto);
        gameOver = true;
    }

    void OnLevelChange()
    {
        EnemySpawnerFemale.ChangeLevel(level);
        EnemySpawnerMale.ChangeLevel(level);
        toNextLevel = Mathf.FloorToInt(toNextLevel*1.5f);
        
        words = sentences[level - 1].Split(' ');
        currlevel = toNextLevel - killed;

        prevTextLength = 0;
        animator.SetTrigger("Level");
    }

    public void EnemyDeath()
    {
        killed++;
        if (killed>toNextLevel)
        {
            level ++;
            OnLevelChange();
        }
        procent = 1 +(float)(killed - toNextLevel) / (float)currlevel;
        percentText();
    }

    private void percentText()
    {
        obj.text = "";
        for (int i = 0; i < words.Length; i++)
        {
            if (procent * words.Length < i) break;
            if(i > prevTextLength)
            {
                prevTextLength = i;
                animator.SetTrigger("Progress");
            }
            obj.text += words[i] + " ";
        }
    }
}
