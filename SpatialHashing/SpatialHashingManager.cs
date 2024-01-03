using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StatialHashing;

namespace SpatialHashing
{
  public class SpatialHashingManager
  {
    private int cols, rows, length, cellSize;
    private Dictionary<int, List<GameObject>> Buckets;
    public SpatialHashingManager(int cellSize)
    {
      this.cellSize = cellSize;
      cols = Constants.WINDOW_WIDTH / cellSize;
      rows = Constants.WINDOW_HEIGHT / cellSize;
      length = cols * rows;
      Buckets = new Dictionary<int, List<GameObject>>();
      for (int i = 0; i < length; i++)
      {
        Buckets.Add(i, new List<GameObject>());
      }
    }
    public void ClearBuckets()
    {
      for(int i = 0; i < length; i++)
      {
        Buckets[i].Clear();
      }
    }

    private void AddCornerToBucket(int X, int Y, List<int> bucketToAddTo)
    {
      int vectorPosition = (int)(
        Math.Floor((double)X / cellSize) +
        // Y/cellSize is multiplied by the amount of cols to get the number of the cell that starts the row
        Math.Floor((double)Y / cellSize) *
        cols 
      );
      // Limiting vector position index
      if (vectorPosition >= length) vectorPosition = length - 1;
      if (vectorPosition < 0) vectorPosition = 0;
      
      if(!bucketToAddTo.Contains(vectorPosition)) bucketToAddTo.Add(vectorPosition);
    }

    public List<int> GetBucketsForAllCorners(GameObject gameObject)
    {
      List<int> bucketsBubbleIsIn = new List<int>();

      // Top left
      AddCornerToBucket(
        gameObject.Rect.X,
        gameObject.Rect.Y,
        bucketsBubbleIsIn
      );
      // Top right
      AddCornerToBucket(
        gameObject.Rect.X + gameObject.Rect.Width,
        gameObject.Rect.Y,
        bucketsBubbleIsIn
      );
      // Bottom left
      AddCornerToBucket(
        gameObject.Rect.X,
        gameObject.Rect.Y + gameObject.Rect.Height,
        bucketsBubbleIsIn
      );
      // Bottom right
      AddCornerToBucket(
        gameObject.Rect.X  + gameObject.Rect.Width,
        gameObject.Rect.Y + gameObject.Rect.Height,
        bucketsBubbleIsIn
      );
      
      return bucketsBubbleIsIn;
    }

    public void RegisterGameObject(GameObject gameObject)
    {
      List<int> bucketList = GetBucketsForAllCorners(gameObject);
      foreach(int bucket in bucketList)
      {
        Buckets[bucket].Add(gameObject);
      }
    }

    public List<GameObject> GetNearby(GameObject gameObject)
    {
      List<GameObject> gameObjectsNearby = new List<GameObject>();
      List<int> bucketIds = GetBucketsForAllCorners(gameObject);
      foreach(int id in bucketIds)
      {
        List<GameObject> bubblesInBucket = Buckets[id].Where(b => b != gameObject).ToList();
        gameObjectsNearby.AddRange(bubblesInBucket);
      }
      return gameObjectsNearby;
    }
  }
}