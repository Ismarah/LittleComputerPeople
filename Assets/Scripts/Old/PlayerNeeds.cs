//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerNeeds : MonoBehaviour {

//    [SerializeField]
//    private Image sleepBar;
//    [SerializeField]
//    private Image hungerBar;
//    [SerializeField]
//    private Image toiletBar;
//    [SerializeField]
//    private Image funBar;

//    [SerializeField]
//    private float hungerDepletion;
//    [SerializeField]
//    private float sleepDepletion;
//    [SerializeField]
//    private float toiletDepletion;
//    [SerializeField]
//    private float funDepletion;

//    [SerializeField]
//    private float hunger = 0;
//    [SerializeField]
//    private float sleep = 0;
//    [SerializeField]
//    private float toilet = 0;
//    [SerializeField]
//    private float boredom = 0;

//    private bool decreasingHunger;
//    private bool decreasingSleep;
//    private bool decreasingToilet;
//    private bool decreasingFun;

//    private bool goingToEatNow;
//    private bool goingToBed;
//    private bool goingToToilet;
//    private bool goingToHaveFun;

//    private IEnumerator havingFun;
//    private IEnumerator onTheToilet;
//    private IEnumerator eating;
//    private IEnumerator sleeping;

//    void Start () {
//        StartCoroutine(DepleteHunger());
//        //StartCoroutine(DepleteSleep());
//        //StartCoroutine(DepleteToilet());
//        //StartCoroutine(DepleteFun());

//        //havingFun = HaveFun(0, 0);
//        //onTheToilet = UseToilet(0, 0);
//        eating = Eat(0, 0);
//        //sleeping = Sleep(0, 0);
//    }

//    private void Update()
//    {
//        hunger = Mathf.Clamp(hunger, 0, 1);
//        sleep = Mathf.Clamp(sleep, 0, 1);
//        toilet = Mathf.Clamp(toilet, 0, 1);
//        boredom = Mathf.Clamp(boredom, 0, 1);

//        //if(hunger >= 0.4f && !goingToEatNow)
//        //{
//        //    EventManager.TriggerEvent("playerNeedsFood");
//        //    goingToEatNow = true;
//        //}

//        //if(toilet >= 0.6f && !goingToToilet)
//        //{
//        //    EventManager.TriggerEvent("playerNeedsToPee");
//        //    goingToToilet = true;
//        //}

//        //if(sleep >= 0.7f && !goingToBed)
//        //{
//        //    EventManager.TriggerEvent("playerIsTired");
//        //    goingToBed = true;
//        //}

//        //if(boredom >= 0.5f && !goingToHaveFun)
//        //{
//        //    EventManager.TriggerEvent("playerIsBored");
//        //    goingToHaveFun = true;
//        //}
//    }

//    private IEnumerator DepleteHunger()
//    {
//        while (!decreasingHunger)
//        {
//            hunger += hungerDepletion / 100 * Time.deltaTime;
//            hungerBar.fillAmount = 1 - hunger;
//            yield return null;
//        }
//    }

//    //private IEnumerator DepleteSleep()
//    //{
//    //    while (!decreasingSleep)
//    //    {
//    //        sleep += sleepDepletion / 100 * Time.deltaTime;
//    //        sleepBar.fillAmount = 1 - sleep; 
//    //        yield return null;
//    //    }
//    //}

//    //private IEnumerator DepleteToilet()
//    //{
//    //    while (!decreasingToilet)
//    //    {
//    //        toilet += toiletDepletion / 100 * Time.deltaTime;
//    //        toiletBar.fillAmount = 1 - toilet; 
//    //        yield return null;
//    //    }
//    //}

//    //private IEnumerator DepleteFun()
//    //{
//    //    while (!decreasingFun)
//    //    {
//    //        boredom += funDepletion / 100 * Time.deltaTime;
//    //        funBar.fillAmount = 1 - boredom;
//    //        yield return null;
//    //    }
//    //}

//    //public void StartHavingFun(float amount, float speed)
//    //{
//    //    havingFun = HaveFun(amount, speed);
//    //    StartCoroutine(havingFun);
//    //}

//    //public void StopHavingFun()
//    //{
//    //    StopCoroutine(havingFun);
//    //    decreasingFun = false;
//    //    goingToHaveFun = false;
//    //    StartCoroutine(DepleteFun());
//    //}

//    //private IEnumerator HaveFun(float amount, float speed)
//    //{
//    //    havingFun = HaveFun(amount, speed);
//    //    decreasingFun = true;
//    //    float currentBoredom = boredom;
//    //    while (boredom > currentBoredom - amount && boredom > 0)
//    //    {
//    //        boredom -= 0.01f * speed * Time.deltaTime;
//    //        funBar.fillAmount = 1 - boredom;
//    //        yield return null;
//    //    }
//    //    decreasingFun = false;
//    //    goingToHaveFun = false;
//    //    StartCoroutine(DepleteFun());
//    //    GetComponent<Player>().ActionFinished();
//    //}

//    //public void StartUsingToilet(float amount, float speed)
//    //{
//    //    onTheToilet = UseToilet(amount, speed);
//    //    StartCoroutine(onTheToilet);
//    //}

//    //public void StopUsingToilet()
//    //{
//    //    StopCoroutine(onTheToilet);
//    //    decreasingToilet = false;
//    //    goingToToilet = false;
//    //    StartCoroutine(DepleteToilet());
//    //}

//    //private IEnumerator UseToilet(float amount, float speed)
//    //{
//    //    onTheToilet = UseToilet(amount, speed);
//    //    decreasingToilet = true;
//    //    float currentToilet = toilet;
//    //    while (toilet > currentToilet - amount && toilet > 0)
//    //    {
//    //        toilet -= 0.01f * speed * Time.deltaTime;
//    //        toiletBar.fillAmount = 1 - toilet;
//    //        yield return null;
//    //    }
//    //    decreasingToilet = false;
//    //    goingToToilet = false;
//    //    StartCoroutine(DepleteToilet());
//    //    GetComponent<Player>().ActionFinished();
//    //}

//    //public void StartSleep(float amount, float speed)
//    //{
//    //    sleeping = Sleep(amount, speed);
//    //    StartCoroutine(sleeping);
//    //}

//    //public void StopSleeping()
//    //{
//    //    StopCoroutine(sleeping);
//    //    decreasingSleep = false;
//    //    goingToBed = false;
//    //    StartCoroutine(DepleteSleep());
//    //}

//    //private IEnumerator Sleep(float amount, float speed)
//    //{
//    //    sleeping = Sleep(amount, speed);
//    //    decreasingSleep = true;
//    //    float currentSleep = sleep;
//    //    while (sleep > currentSleep - amount && sleep > 0)
//    //    {
//    //        sleep -= 0.01f * speed * Time.deltaTime;
//    //        sleepBar.fillAmount = 1 - sleep;
//    //        yield return null;
//    //    }
//    //    decreasingSleep = false;
//    //    goingToBed = false;
//    //    StartCoroutine(DepleteSleep());
//    //    GetComponent<Player>().ActionFinished();
//    //}

//    public void StartEating(float amount, float speed)
//    {
//        Debug.Log("Start eating");
//        eating = Eat(amount, speed);
//        StartCoroutine(eating);
//    }

//    public void StopEating()
//    {
//        Debug.Log("Stop eating");
//        StopCoroutine(eating);
//        goingToEatNow = false;
//        decreasingHunger = false;
//        StartCoroutine(DepleteHunger());
//    }

//    private IEnumerator Eat(float amount, float speed)
//    {
//        eating = Eat(amount, speed);
//        decreasingHunger = true;

//        float currentHunger = hunger;
//        if(amount > currentHunger)
//        {
//            amount = currentHunger;
//        }

//        while (hunger > currentHunger - amount && hunger > 0)
//        {
//            Debug.Log("Eating");
//            hunger -= 0.01f  * speed * Time.deltaTime;
//            hungerBar.fillAmount = 1 - hunger;
//            yield return null;
//        }
//        goingToEatNow = false;
//        decreasingHunger = false;
//        StartCoroutine(DepleteHunger());
//        GetComponent<Player>().ActionFinished();
//    }
//}
