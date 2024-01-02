using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpatialHashing
{
  public class SpatialHashingManager
  {
    private int cols, rows, length, cellSize;
    private Dictionary<int, List<Bubble>> Buckets;
    public SpatialHashingManager(int cellSize)
    {
      this.cellSize = cellSize;
      cols = Constants.WINDOW_WIDTH / cellSize;
      rows = Constants.WINDOW_HEIGHT / cellSize;
      length = cols * rows;
      Buckets = new Dictionary<int, List<Bubble>>();
      for (int i = 0; i < length; i++)
      {
        Buckets.Add(i, new List<Bubble>());
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

    public List<int> GetBucketsForAllCorners(Bubble bubble)
    {
      List<int> bucketsBubbleIsIn = new List<int>();

      // Top left
      AddCornerToBucket(
        bubble.Rect.X,
        bubble.Rect.Y,
        bucketsBubbleIsIn
      );
      // Top right
      AddCornerToBucket(
        bubble.Rect.X + bubble.Rect.Width,
        bubble.Rect.Y,
        bucketsBubbleIsIn
      );
      // Bottom left
      AddCornerToBucket(
        bubble.Rect.X,
        bubble.Rect.Y + bubble.Rect.Height,
        bucketsBubbleIsIn
      );
      // Bottom right
      AddCornerToBucket(
        bubble.Rect.X  + bubble.Rect.Width,
        bubble.Rect.Y + bubble.Rect.Height,
        bucketsBubbleIsIn
      );
      
      return bucketsBubbleIsIn;
    }

    public void RegisterBubble(Bubble bubble)
    {
      List<int> bucketList = GetBucketsForAllCorners(bubble);
      foreach(int bucket in bucketList)
      {
        Buckets[bucket].Add(bubble);
      }
    }

    public List<Bubble> GetNearby(Bubble bubble)
    {
      List<Bubble> bubblesNearby = new List<Bubble>();
      List<int> bucketIds = GetBucketsForAllCorners(bubble);
      foreach(int id in bucketIds)
      {
        List<Bubble> bubblesInBucket = Buckets[id].Where(b => b != bubble).ToList();
        bubblesNearby.AddRange(bubblesInBucket); 
        /*
        The bubble passed in the argument would be included in the list.
        Shouldn't it be removed?
        Or shoudn't at least check in each collision loop that I'm not
        checking collision between one bubble against itself?
        */
      }
      return bubblesNearby;
    }
  }
}