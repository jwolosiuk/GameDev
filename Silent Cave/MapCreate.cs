using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour {
    public float UPPER_BOUND = 1.5f;
    float LOWER_BOUND = 0;

    float cameraOffset = 0;
    float caveStartX = 0;

    float MAP_END_Y = 7;
    float MAP_BEGINNING_Y = -7;

    float MAP_WIDTH;
    float MAP_WIDTH_APPEND = 1;
    float MAP_HEIGHT;
    float MAP_DEPTH = 1;
    float NARROWING_HEIGHT = 2f;
    float NARROWING_DEPTH = 1f;


    float DISTANCE = 0.1f;
    int SAMPLING = 1;

    int BUFOR = 20;

    float textureWidth;
    float mirror=1;
    float textureX = 0;

    Vector3 lowerOffset;
    Vector3 depperOffset;
    Vector3 upperOffset;

    Vector3 firstToMiddleLowOffset;
    Vector3 firstToMiddleUpOffset;
    Vector3 firstToUpOffset;

    Sounder sounder;
    GameObject ceilingObject;
    GameObject floorObject;
    GameObject narrowingObject;

    ObstacleSpawner spawner;

    bool generateCave = false;

    float time = 0;

    List<Vector3> listOfVertices;
    List<Vector3> listOfUV;
    float[] array;

    float phasa = 0;

    void StartGenerating(EventInfoS e)
    {
        generateCave = true;
    }

    void StopGenerating(EventInfoS e)
    {
        generateCave = false;
    }

    void Start() {
        GameManager.eventSystem.Subscribe("GameInitialized", StartGenerating);
        GameManager.eventSystem.Subscribe("PlayerDied", StopGenerating);
        MAP_WIDTH = (Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 5)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 5))).x + MAP_WIDTH_APPEND;

        SAMPLING = (int)((MAP_WIDTH * 2) / DISTANCE);
        lowerOffset = new Vector3(0, MAP_BEGINNING_Y, 0);
        upperOffset = new Vector3(0, MAP_END_Y, 0);
        depperOffset = new Vector3(0, 0, MAP_DEPTH);

        ceilingObject = transform.FindChild("Ceiling").gameObject;
        floorObject = transform.FindChild("Floor").gameObject;
        narrowingObject = transform.FindChild("Narrowing").gameObject;

        MAP_HEIGHT = ceilingObject.transform.position.y - floorObject.transform.position.y;
        firstToMiddleLowOffset = depperOffset + new Vector3(0, (MAP_HEIGHT - NARROWING_HEIGHT) / 2, NARROWING_DEPTH);
        firstToMiddleUpOffset = depperOffset + new Vector3(0, (MAP_HEIGHT + NARROWING_HEIGHT)/2, NARROWING_DEPTH);
        firstToUpOffset = depperOffset + new Vector3(0, MAP_HEIGHT, 0);

        spawner = gameObject.GetComponentInChildren<ObstacleSpawner>();
        
        array = GenerateSinToArray(SAMPLING, 12,phasa);
        Debug.Log("sampling");
        Debug.Log(SAMPLING);
        Debug.Log(array.Length);
        phasa += 12;
        sounder = GameManager.mic;

        textureWidth = 10;

        CreateVerticesFromArray(array);
        UpdateFloorMesh();
        UpdateCeilingMesh();
        UpdateNarrowingMesh();
    }

    float[] AddArrays(float[] A, float[] B)
    {
        int size = Mathf.Min(A.Length, B.Length);
        for (int i = 0; i < size; i++)
        {
            A[i] += B[i];
            A[i] /= 2;
        }
        return A;
    }

    float[] GenerateSinToArray(int samples, float freq, float phaze)
    {
        array = new float[samples];
        int i = 0;
        for (i = 0; i < array.Length; i++)
        {
            array[i] = Mathf.Sin(freq * i / SAMPLING + phaze);
        }
        return array;
    }

    void AddVerticesFromArray(float[] points)
    {
        float offsetPos = listOfVertices[listOfVertices.Count - 1].x;
        float posY;
        float posZ = 0;
        float posX = offsetPos;
        for (int i = 0; i < points.Length; i++)
        {
            posY = (points[i] + 1) / 2 * (UPPER_BOUND - LOWER_BOUND) - LOWER_BOUND;
            posX += DISTANCE;
            if (spawner.ShouldGenerateHere(posX+transform.position.x))
            {
                spawner.GenerateObstacleHere(posX+transform.position.x, posY, posY + MAP_HEIGHT);
            }
            listOfVertices.Add(new Vector3(posX, posY, posZ));
        }
    }

    void RemoveVerticesFromBeggining(int toRemove)
    {
        listOfVertices.RemoveRange(0, toRemove);
        spawner.RemoveSomeLastObstacles(caveStartX);
    }

    void CreateVerticesFromArray(float[] points)
    {
        List<Vector3> vectors = new List<Vector3>();
        float posX = 0, posY = 0, posZ = 0;
        for (int i = 0; i < points.Length; i++)
        {
            posY = (points[i] + 1) / 2 * (UPPER_BOUND - LOWER_BOUND) - LOWER_BOUND;
            posX += DISTANCE;
            vectors.Add(new Vector3(posX, posY, posZ));
        }
        listOfVertices = vectors;
    }

    float GetTextureX(float posX)
    {
        float t = (posX / textureWidth);
        int ile = (int)t;
        t = t - ile;
        if (ile % 2 == 0) return t;
        return 1-t;
    }

    Mesh CreateNarrowingMesh()
    {
        Mesh narrowing = new Mesh();
        List<Vector3> narrowingVertices = new List<Vector3>();
        List<int> narrowingTriangles = new List<int>();
        List<Vector2> narrowingUV = new List<Vector2>();

        Vector3 first = listOfVertices[0];

        narrowingVertices.Add(first + depperOffset);
        narrowingVertices.Add(first + firstToMiddleLowOffset);
        narrowingVertices.Add(first + firstToMiddleLowOffset);
        narrowingVertices.Add(first + firstToMiddleUpOffset);
        narrowingVertices.Add(first + firstToMiddleUpOffset);
        narrowingVertices.Add(first + firstToUpOffset);

        narrowingUV.Add(new Vector2(textureX,0));
        narrowingUV.Add(new Vector2(textureX, 0.3f));
        narrowingUV.Add(new Vector2(textureX, 0.3f));
        narrowingUV.Add(new Vector2(textureX, 0.7f));
        narrowingUV.Add(new Vector2(textureX, 0.7f));
        narrowingUV.Add(new Vector2(textureX, 1));

        textureX = GetTextureX(first.x);
        for (int i = 1, triInd = 6; i < listOfVertices.Count; i++, triInd += 2)
        {
            first = listOfVertices[i];
            textureX = GetTextureX(first.x);
            narrowingVertices.Add(first + depperOffset);
            narrowingVertices.Add(first + firstToMiddleLowOffset);
            narrowingVertices.Add(first + firstToMiddleLowOffset);
            narrowingVertices.Add(first + firstToMiddleUpOffset);
            narrowingVertices.Add(first + firstToMiddleUpOffset);
            narrowingVertices.Add(first + firstToUpOffset);

            narrowingUV.Add(new Vector2(textureX, 0));
            narrowingUV.Add(new Vector2(textureX, 0.3f));
            narrowingUV.Add(new Vector2(textureX, 0.3f));
            narrowingUV.Add(new Vector2(textureX, 0.7f));
            narrowingUV.Add(new Vector2(textureX, 0.7f));
            narrowingUV.Add(new Vector2(textureX, 1));

            int[] nodes = { triInd - 6, triInd-5, triInd, triInd + 1, triInd, triInd -5};
            narrowingTriangles.AddRange(nodes);
            triInd += 2;
            nodes = new int[] { triInd - 6, triInd - 5, triInd, triInd + 1, triInd, triInd - 5 };
            narrowingTriangles.AddRange(nodes);
            triInd += 2;
            nodes = new int[] { triInd - 6, triInd - 5, triInd, triInd + 1, triInd, triInd - 5 };
            narrowingTriangles.AddRange(nodes);
        }
        narrowing.vertices = narrowingVertices.ToArray();
        narrowing.triangles = narrowingTriangles.ToArray();
        narrowing.uv = narrowingUV.ToArray();
        return narrowing;
    }

    Mesh CreateCeilingMesh()
    {
        Mesh ceiling = new Mesh();
        List<Vector3> ceilingVertices = new List<Vector3>();
        List<int> ceilingTriangles = new List<int>();

        Vector3 first = listOfVertices[0];

        ceilingVertices.Add(first);
        ceilingVertices.Add(first + depperOffset);
        ceilingVertices.Add(first + upperOffset);

        for (int i = 1, triInd = 3; i < listOfVertices.Count; i++, triInd += 3)
        {
            first = listOfVertices[i];

            ceilingVertices.Add(first);
            ceilingVertices.Add(first + depperOffset);
            ceilingVertices.Add(first + upperOffset);

            int[] nodes = { triInd - 3, triInd, triInd - 2, triInd + 1, triInd - 2, triInd };
            ceilingTriangles.AddRange(nodes);
            nodes = new int[] { triInd + 2, triInd, triInd - 1, triInd, triInd - 3, triInd - 1 };
            ceilingTriangles.AddRange(nodes);
        }
        ceiling.vertices = ceilingVertices.ToArray();
        ceiling.triangles = ceilingTriangles.ToArray();
        
        return ceiling;
    }

    Mesh CreateFloorMesh()
    {
        Mesh floor = new Mesh();
        List<Vector3> floorVertices = new List<Vector3>();
        List<int> floorTriangles = new List<int>();

        Vector3 first = listOfVertices[0];

        floorVertices.Add(first);
        floorVertices.Add(first + depperOffset);
        floorVertices.Add(first + lowerOffset);

        for (int i = 1, triInd = 3; i < listOfVertices.Count; i++, triInd += 3)
        {
            first = listOfVertices[i];

            floorVertices.Add(first);
            floorVertices.Add(first + depperOffset);
            floorVertices.Add(first + lowerOffset);

            int[] nodes = { triInd - 3, triInd - 2, triInd, triInd + 1, triInd, triInd - 2 };
            floorTriangles.AddRange(nodes);
            nodes = new int[] { triInd + 2, triInd - 3, triInd, triInd + 2, triInd - 1, triInd - 3 };
            floorTriangles.AddRange(nodes);
        }
        floor.vertices = floorVertices.ToArray();
        floor.triangles = floorTriangles.ToArray();
        return floor;
    }

    void UpdateNarrowingMesh()
    {
        Mesh narrowing = CreateNarrowingMesh();
        narrowing.RecalculateNormals();
        narrowingObject.GetComponentInChildren<MeshFilter>().mesh = narrowing;
        narrowingObject.GetComponentInChildren<MeshCollider>().sharedMesh = narrowing;
    }

    void UpdateCeilingMesh()
    {
        Mesh ceiling = CreateCeilingMesh();
        ceiling.RecalculateNormals();
        ceilingObject.GetComponentInChildren<MeshFilter>().mesh = ceiling;
        ceilingObject.GetComponentInChildren<MeshCollider>().sharedMesh = ceiling;
    }

    void UpdateFloorMesh()
    {
        Mesh floor = CreateFloorMesh();
        floor.RecalculateNormals();
        floorObject.GetComponentInChildren<MeshFilter>().mesh = floor;
        floorObject.GetComponentInChildren<MeshCollider>().sharedMesh = floor;
    }

    void Update() {
        time = GameManager.instance.gameTime;
        if (generateCave)
        {
            cameraOffset = GameManager.player.transform.position.x;
            if (cameraOffset - caveStartX > BUFOR * DISTANCE)
            {
                cameraOffset -= BUFOR * DISTANCE;
                array = GenerateSinToArray(BUFOR, 12, phasa);
                phasa += (float)(BUFOR) * 12 / SAMPLING;
                array = AddArrays(array, sounder.GetLastSound(BUFOR));
                AddVerticesFromArray(array);
                RemoveVerticesFromBeggining(BUFOR);
                caveStartX += BUFOR * DISTANCE;
                UpdateCeilingMesh();
                UpdateFloorMesh();
                UpdateNarrowingMesh();
            }
        }
    }



    
}
