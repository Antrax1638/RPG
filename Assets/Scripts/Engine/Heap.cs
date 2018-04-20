using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
	public Heap(int size)
	{
		Items = new T[size];
	}

	public void Add(T item)
	{
		item.HeapIndex = Count;
		Items [Count] = item;
		SortUp (item);
		Count++;
	}

	public T RemoveFirst()
	{
		T FirstItem = Items [0];
		Count--;
		Items [0] = Items [Count];
		Items [0].HeapIndex = 0;
		SortDown (Items [0]);
		return FirstItem;
	}

	public bool Contains(T item)
	{
		return Equals (Items [item.HeapIndex], item);
	}
		
	public int GetCount()
	{
		return Count;
	}

	public void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true)
		{
			T Parent = Items [parentIndex];
			if (item.CompareTo (Parent) > 0) 
				Swap (item, Parent);
			else
				break;

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	public void SortDown (T item)
	{
		while (true) 
		{
			int ChildIndexLeft = item.HeapIndex * 2 + 1;
			int ChildIndexRight = item.HeapIndex * 2 + 1;
			int SwapIndex = 0;

			if (ChildIndexLeft < Count) {
				SwapIndex = ChildIndexLeft;

				if (ChildIndexRight < Count)
				if (Items [ChildIndexLeft].CompareTo (Items [ChildIndexRight]) < 0)
					SwapIndex = ChildIndexRight;

				if (item.CompareTo (Items [SwapIndex]) < 0)
					Swap (item, Items [SwapIndex]);
				else
					return;
			} else
				return;
		}
	}

	void Swap(T itemA, T itemB)
	{
		Items [itemA.HeapIndex] = itemB;
		Items [itemB.HeapIndex] = itemA;
		int ItemIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = ItemIndex;
	}
		
	//Variables:
	public int Length{get{ return Items.Length; }}
	T[] Items;
	int Count = 0;
}

public interface IHeapItem<T> : System.IComparable<T>
{
	int HeapIndex{ set; get;}

}
