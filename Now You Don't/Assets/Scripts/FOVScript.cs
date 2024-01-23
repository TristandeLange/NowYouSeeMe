using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FOVScript : MonoBehaviour
{
    //FOV cannot be attached to another game object (at least one thats not at the origin.
    [SerializeField] private LayerMask layerMask;
    
    public float fov;       //previously was 90f                Stands for FOV
    public int rayCount;    //previously was 10                 How many Ray's we will send out, and also how many triangles will be made
    public float viewDistance;     //previously was 20f         The distance the mesh and rays will travel in total

    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;

    private bool powered = true;

    // Start is called before the first frame update
    private void Start()
    {
        //Lets go through the steps
        mesh = new Mesh();                                      //Make a new mesh             
        GetComponent<MeshFilter>().mesh = mesh;                 //Assing the meshFilter on the object to have this new mesh
        origin = Vector3.zero;
        this.gameObject.transform.position = origin;
        layerMask = ~layerMask;

    }

    
    private void LateUpdate()
    {
        if ( powered ) 
        {
            float angle = startingAngle;                            //this is the angle of the view,  we divide by two so that the player object is in the middle
            float angleIncrease = fov / rayCount;                   //How much to increase the angle by depending on the amount of rays. So fov=90 and raycount=10 we have to increase the angle by 90 degrees each time


            Vector3[] vertices = new Vector3[rayCount + 1 + 1];     //vertices + 1 for origin and + 1 for ray 0
            Vector2[] uv = new Vector2[vertices.Length];            //Always the same as vertices.length
            int[] triangles = new int[rayCount * 3];                //the amount of rays times 3 because we need 3 values per triangle

            vertices[0] = origin;                                   //self explanatory, any ray starts at the origin

            int vertexIndex = 1;                                    //set vertex Index to 1, because we've already dealt with the 0th case about
            int triangleIndex = 0;                                  //Triangle index starts at 0 because we've only determined the first point, not the first triangle



            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);   //draws a ray from the origin to the determined angle, at the specified distance
                if (raycastHit.collider == null)                     //returns true if nothing was hit
                {
                    //no collision
                    vertex = origin + GetVectorFromAngle(angle) * viewDistance;     //if there was no collision, draw the line to the full length
                }
                else
                {
                    //collision
                    vertex = raycastHit.point;                      //if there was a hit, draw a triangle until that point
                    if (raycastHit.collider.name == "Player")       //determines if the rays ever collide with the player
                    {
                        raycastHit.transform.SendMessage("HitByRay");   //Sends a message to the player object that triggers a function
                    }
                }

                vertices[vertexIndex] = vertex;                     //we assign the current ray's farthest vertice with the previously determined point

                if (i > 0)                                          //since we are dealing with a -1 vertexIndex scenario, there won't be a previous ray on the 0th run and we are already accounting for the origin
                {
                    triangles[triangleIndex + 0] = 0;               //starts at the origin
                    triangles[triangleIndex + 1] = vertexIndex - 1; //the previous ray
                    triangles[triangleIndex + 2] = vertexIndex;     //the new ray we just determined

                    triangleIndex += 3;                             //increment by 3 as we have the 3 points of the next triangle
                }

                vertexIndex++;                                      //increment vertex index to get the next line
                angle -= angleIncrease;                             //decrement the angle so that we aren't drawing the same line over and over, and - because we want it running clockwise
            }

            // old base stuff for drawing a mesh triangle
            //triangles[0] = 0;
            //triangles[1] = 1;
            //triangles[2] = 2;


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
        }
        else 
        {
            mesh.Clear();
        }
        
    }

    public void setPowered(bool IO) 
    {
        this.powered = IO;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRadians = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }

    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void setOrigin(Vector3 origin)                               //public method for setting the origin
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)                   //public method to set the aim direction
    {
        startingAngle = GetAngleFromVectorFloat (aimDirection) + fov/2; //doing + fov/2 so that the fov is centered on the player character 
    }

}
