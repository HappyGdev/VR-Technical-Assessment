//using UnityEngine;

//public class ExtraLogicB : MonoBehaviour
//{
//    void Start()
//    {
//        InvokeRepeating(nameof(Tick), 0.5f, 0.02f);
//    }

//    void Tick()
//    {
//        foreach (var g in MainClass.ALL)
//        {
//            if (g == null) continue;

//            if (g.TryGetComponent(out RandomThings rt))
//            {
//                rt.doRotate();
//            }
//        }
//    }
//}
