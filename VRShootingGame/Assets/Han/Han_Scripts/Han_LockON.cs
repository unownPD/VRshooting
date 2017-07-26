using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_LockON : MonoBehaviour {

    //스크린
    //public GameObject screen;

    //lockon 이미지
    public GameObject Lock_ONimage;
    //lockon원래 크기
    Vector3 originScale;
    //보정값
    public float value = 0.3f;


    //시간 잠시
    float currentTime;
    public float LockOnTime;

    //록온된 오브젝트들
    public List<GameObject> Lock_ONobj = new List<GameObject>();

    public List<GameObject> Lock_ONimg = new List<GameObject>();

    public List<Han_enemyHP> EnemysHP = new List<Han_enemyHP>();

    // Use this for initialization
    void Start ()
    {
        //기본 크기 저장
        originScale = Lock_ONimage.transform.localScale;
        //처음에는 안보이게
        Lock_ONimage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //레이는 기본
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, 1000, 1 << LayerMask.NameToLayer("Layer_enemy")))
        {
            //적 HP 알아보기
            Han_enemyHP enemyHPscript = hitinfo.transform.gameObject.GetComponent<Han_enemyHP>();

            // HP가 0 이상이면
            if (enemyHPscript.NowHP > 0)
            {

                //록온image 활성화
                Lock_ONimage.SetActive(true);

                //방향을 구하고
                //Vector3 dir = hitinfo.point - transform.position;

                //록온의 이미지는 맞은 위치
                Lock_ONimage.transform.position = hitinfo.point;

                //록온 이미지 크기를 거리에 맞춰 조절
                Lock_ONimage.transform.localScale = originScale * hitinfo.distance * value;

                currentTime += Time.deltaTime;

                //지금은 a를 누르면 나중에는 터치로 바꿀 것임
                if (currentTime > LockOnTime)
                {
                    //록온된 유닛 리스트에 추가
                    Lock_ONobj.Add(hitinfo.transform.gameObject);

                    //록온된 유닛의 HP 스크립트도 리스트에 추가
                    EnemysHP.Add(enemyHPscript);

                    //록온 이미지 생산
                    GameObject Lockimg = Instantiate(Lock_ONimage);

                    //록온 이미지 리스트에 추가(록온된 유닛과 록온 이미지가 1:1 매칭
                    Lock_ONimg.Add(Lockimg);

                    //록온된 유닛 중복 체크
                    //count가 2 이상이 되면
                    if (Lock_ONobj.Count > 1)
                    {
                        //0번 부터 막 리스트에 들어간 obj전 까지 검사한다
                        for (int i = 0; i < Lock_ONobj.Count - 1; i++)
                        {
                            //막 들어간 obj가 리스트에 이미 있다면
                            if (Lock_ONobj[Lock_ONobj.Count - 1] == Lock_ONobj[i])
                            {
                                //막 들어간 obj를 리스트에서 삭제
                                Lock_ONobj.RemoveAt(Lock_ONobj.Count - 1);

                                //막 들어간 imj를 리스트에서 삭제 원본도 삭제
                                Destroy(Lock_ONimg[Lock_ONobj.Count - 1]);
                                Lock_ONimg.RemoveAt(Lock_ONobj.Count - 1);

                                //스크립트도 리스트에서 삭제
                                EnemysHP.RemoveAt(Lock_ONobj.Count - 1);
                            }
                        }
                    }
                }
            }
        }

        //ray가 맞고 있지 않다면 비활성화
        else
        {
            Lock_ONimage.SetActive(false);

            currentTime = 0;
        }

        //록온 타겟이 0개 초과면
        if (Lock_ONobj.Count > 0)
        {
            for (int i = 0; i < Lock_ONobj.Count; i++)
            {
                //록온 타겟이 없어지면
                if (EnemysHP[i].NowHP  <= 0)
                {
                    //록온 오브젝트 리스트에서 삭제
                    Lock_ONobj.RemoveAt(i);

                    //록온 이미지 삭제
                    Destroy(Lock_ONimg[i]);

                    //록온 이미지 리스트에서 삭제
                    Lock_ONimg.RemoveAt(i);

                    //적 스크립트 리스트에서 삭제
                    EnemysHP.RemoveAt(i);
                }
                else
                {
                    //록온 이미지는 록온된 오브젝트에 있는다
                    Lock_ONimg[i].transform.position = Lock_ONobj[i].transform.position;
                    //록온 이미지가 바라보는 방향은 오브젝트가 나의 사이 벡터 
                    Lock_ONimg[i].transform.forward = transform.position - Lock_ONobj[i].transform.position;
                    //거리에 따라 변화 이건 적용 할지 안할지 고민
                    Lock_ONimg[i].transform.localScale = originScale * Vector3.Distance(transform.position,Lock_ONobj[i].transform.position) * value;
                }
            }
        }
    }
}
