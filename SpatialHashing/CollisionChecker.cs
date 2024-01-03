using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StatialHashing;

namespace SpatialHashing
{
  public interface CollisionCheckerIteratorStrategy
  {
    public int CollisionCheckCounter { get { return CollisionCheckCounter;}}
    public void UpdateCollisions(List<GameObject> gameObjectList);
  };

  public class NormalCollisionCheckerIterator: CollisionCheckerIteratorStrategy
  {
    private int collisionCheckCounter = 0;
    public int CollisionCheckCounter { get { return collisionCheckCounter;} }
    public void UpdateCollisions(List<GameObject> gameObjectList)
    {
      collisionCheckCounter = 0;
      for(int i = 0; i < Constants.BUBBLE_AMOUNT; i++)
      {
        collisionCheckCounter++;
        for(int j = i+1; j < Constants.BUBBLE_AMOUNT; j++)
        {
          collisionCheckCounter++;
          CollisionChecker.CheckCollision(gameObjectList[i], gameObjectList[j]);
        }
      }
    }
  }

  public class SpatialHashingCollisionCheckerIterator: CollisionCheckerIteratorStrategy
  {
    private int collisionCheckCounter = 0;
    public int CollisionCheckCounter { get { return collisionCheckCounter;} }
    private SpatialHashingManager spatialHashingManager;
    
    // Constructor
    public SpatialHashingCollisionCheckerIterator(int cellSize)
    {
      spatialHashingManager = new SpatialHashingManager(cellSize);
    }

    public void UpdateCollisions(List<GameObject> gameObjectList)
    {
      collisionCheckCounter = 0;
      foreach(GameObject gameObject in gameObjectList)
      {
        spatialHashingManager.RegisterGameObject(gameObject);
      }

      for(int i = 0; i < Constants.BUBBLE_AMOUNT; i++)
      {
        collisionCheckCounter++;
        List<GameObject> nearbyGameObjects = spatialHashingManager.GetNearby(gameObjectList[i]);
        foreach(Bubble nearbyGameObject in nearbyGameObjects)
        {
          collisionCheckCounter++;
          CollisionChecker.CheckCollision(gameObjectList[i], nearbyGameObject);
        }
      }

      spatialHashingManager.ClearBuckets();
    }
  }

  public class CollisionChecker
  {
    static public void CheckCollision(GameObject gameObject1, GameObject gameObject2)
    {
      if (
        gameObject1.Rect.X < gameObject2.Rect.X + gameObject2.Rect.Width &&
        gameObject1.Rect.X + gameObject1.Rect.Width > gameObject2.Rect.X &&
        gameObject1.Rect.Y < gameObject2.Rect.Y + gameObject2.Rect.Height &&
        gameObject1.Rect.Y + gameObject1.Rect.Height > gameObject2.Rect.Y
      )
      {
        gameObject1.OnCollision();
        gameObject2.OnCollision();
      }
    }
  }
}