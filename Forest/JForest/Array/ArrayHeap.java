package forest.array;

import forest.Message;
import java.util.Stack;
import java.util.Comparator;

// minHeap by default
public class ArrayHeap<T> {


	// private fields
	private Comparator<Object> comparator;
	private Object[] arr;

	private int size;
	private int capacity;


	// private constant
	private static final int DEFAULT_CAPACITY = 10;


	// shared private helper
	// call this before incrementing array size
	private void expand() {
		if (size==capacity) {
			Object[] newArr = new Object[capacity*2];
			for (int i=0; i<size; ++i) {
				newArr[i]=arr[i];
			}
			arr=newArr;
		}
	}


	// primitive accessor 
	public int getSize() {
		return size;
	}

	public boolean contains(T obj) {
		if (size==0) {
			return false;
		} else {
			for (int i=0; i<size; ++i) {
				if (comparator.compare(obj, arr[i])==0) {
					return true;
				} 
			}
			return false;
		}
	}

	public T peek() throws NullPointerException {
		if (size==0) {
			throw new NullPointerException(Message.emptyTree);
		} else {
			Object obj = arr[0];
			return (T)obj;
		}
	}

	public boolean isEmpty() {
		return size==0;
	}


	// primitive mutator
	public void trimToSize() {
		if (size>=1) {
			Object[] newArr = new Object[size];

			for (int i=0; i<size; ++i) {
				newArr[i]=arr[i];
			}

			arr=newArr;
		} 
	}

	public void clear() {
		for (int i=0; i<size; ++i) {
			arr[i]=null;
		}
		size=0;
	}


	// push and pop

	private void percolateUp(int pos) {

		Object temp = arr[pos];

		while (pos>0) {
			int posDiv2 = pos/2; 
			int parentPosition =  posDiv2*2 == pos ? posDiv2-1 : posDiv2;

			if (comparator.compare(temp,arr[parentPosition])<0) {
				arr[pos]=arr[parentPosition];
				pos=parentPosition;
			} else {
				arr[pos]=temp;
				return;
			}
		}
	}

	private void percolateDown(int pos) {

		Object temp = arr[pos];

		for (int child = pos; child * 2 < size; pos = child) {
			child=pos*2+1; //left
			if (child!=size && comparator.compare(arr[child+1],arr[child])<0) {
				// right exists and smaller than left
				++child;
			}

			if (comparator.compare(temp,child)>0) { 
				arr[pos]=arr[child];
			} else {
				break;
			}
		}

		arr[pos]=temp;

	}

	public void push(T obj) {
		expand();

		arr[size]=obj;
		
		percolateUp(size);

		++size;
	}

	public T pop() throws NullPointerException {
		if (size==0) {
			throw new NullPointerException(Message.emptyTree);
		}

		Object toReturn = arr[0];
		arr[0] = arr[--size];
		arr[size] = null;

		percolateDown(0);

		return (T)toReturn;
	}


	// constructors
	public ArrayHeap(Comparator<Object> comparator) {
		this(comparator,DEFAULT_CAPACITY);
	}

	public ArrayHeap(Comparator<Object> comparator, int capacity) {
		this.size = 0;
		this.capacity = capacity < 1 ? DEFAULT_CAPACITY : capacity;
		this.comparator = comparator;

		arr = new Object[capacity];
	}

}