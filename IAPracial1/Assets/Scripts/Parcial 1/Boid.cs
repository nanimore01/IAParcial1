using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Boid : MonoBehaviour
{
    public float separationRadius;
    public float viewRadius;
    public float radiusBoid;
    public float maxSpeed;
    public float maxForce;
    bool _escape;
    bool _evading;
    

    [Range(0, 4)]
    public float weightSeparation, weightAlignment, weightCohesion;

    public Vector3 velocity;

    private void Start()
    {
        AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
        GameManager.instance.boids.Add(this);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, GameManager.instance.cazador.transform.position) < radiusBoid)
        {
            GameManager.instance.cazador.target = null;
            GameManager.instance.boids.Remove(this);
            gameObject.SetActive(false);    
        }


        if (Vector3.Distance(transform.position, GameManager.instance.cazador.transform.position) <= viewRadius)
        {
            print("Detecto Cazador");
            AddForce(Evade((GameManager.instance.cazador.transform.position + GameManager.instance.cazador.velocity)));
            _evading = true;
        }
        else if (Vector3.Distance(transform.position, GameManager.instance.cazador.transform.position) >= viewRadius && _evading)
        {
            print(_escape);
            _escape = true;
            _evading = false;
        }
        
        if(Vector3.Distance(transform.position, GameManager.instance.food.transform.position) < (viewRadius * 5) && GameManager.instance.food.isActiveAndEnabled)
        {
            AddForce(Arrive(GameManager.instance.food.transform.position));
        }
        else
        {
            Flocking();
        }
        
        if(_escape) 
        {
            AddForce(new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)));
            _escape = false;
        }
        
        
        
            

        transform.position = GameManager.instance.ApplyBounds(transform.position + velocity * Time.deltaTime);
        transform.forward = velocity;
    }

    public void Flocking()
    {
        AddForce(Separation(GameManager.instance.boids, separationRadius) * weightSeparation);
        AddForce(Alignment(GameManager.instance.boids) * weightAlignment);
        AddForce(Cohesion(GameManager.instance.boids) * weightCohesion);
    }
    
    Vector3 Separation(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in boids)
        {
            var dir = item.transform.position - transform.position;
            if (dir.magnitude > radius || item == this)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 Alignment(List<Boid> boids)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in boids)
        {
            if (item == this) continue;

            if (Vector3.Distance(transform.position, item.transform.position) <= viewRadius)
            {
                desired += item.velocity;

                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion(List<Boid> boids)
    {
        Vector3 desired = transform.position;
        int count = 0;

        foreach (var item in boids)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist > viewRadius || item == this) continue;

            desired += item.transform.position;
            count++;
        }

        if (count == 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }
    
    public void AddForce(Vector3 dir)
    {
        velocity += dir;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    Vector3 Seek(Vector3 dir)
    {
        return Vector3.ClampMagnitude(((dir - transform.position).normalized * maxSpeed) - velocity, maxForce);
    }
    Vector3 Arrive(Vector3 dir)
    {
        float dist = Vector3.Distance(transform.position, dir);

        if (dist > viewRadius)
            return Seek(dir);

        var desired = dir - transform.position;
        desired.Normalize();
        desired *= (maxSpeed * (dist / viewRadius));

        var steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }
    Vector3 Evade(Vector3 dir)
    {
        return Flee(dir);
    }

    Vector3 Flee(Vector3 dir)
    {
        return -Seek(dir);
    }

    
}
