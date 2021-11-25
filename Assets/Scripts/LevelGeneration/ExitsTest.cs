using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitsTest : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject roomPrefab;

    [SerializeField] float roomSpacing;

    void Start()
    {
        LevelLayoutGenerator lLG = GetComponent<LevelLayoutGenerator>();
        /*QuaternaryTree<Exits> layout = lLG.GenerateLayout(30,10);
        void TreeTraversal(GameObject go, QuaternaryTree<Exits> tree)
        {
            if(!tree.LeftSon.IsEmpty)
            {
                
            }
        }*/
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
